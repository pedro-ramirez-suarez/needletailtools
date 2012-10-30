using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccess.Migrations.Model;
using System.IO;
using Microsoft.SqlServer.Management.Smo;
using System.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;
using System.Configuration;

namespace Needletail.DataAccess.Migrations
{
    /// <summary>
    /// This will run scripts related with DB creationg and DB Migrations
    /// All Scripts should be placed under a "Migrations" folder.
    /// Script to create initial DB and/or seed the database should be named "InitializeDatabase.sql"
    /// Patches/Migrations should be named "Patch_xxx.sql" where xxx is an integer indicating the order on which the scripts should be ran
    /// REMOVE ANY "GO" STATEMENT IN YOUR SCRIPTS
    /// You need to add the following in your app.config or web.config file
    /// <startup useLegacyV2RuntimeActivationPolicy="true">
    ///     <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.0"/>
    /// </startup>
    /// Or the framework version that you are running against
    /// </summary>
    public class Migrator
    {
        private static string _ConnectionStringName {get; set;}
        private static string _ConnectionString { get; set; }
        private static MigrationContext Context { get; set; }


        public static void Migrate(string connectionString,string path="")
        {
            _ConnectionStringName = connectionString;
            Context = new MigrationContext(connectionString);
            _ConnectionString = ConfigurationManager.ConnectionStrings[connectionString].ConnectionString;
            //Create the Migrations table if does not exists
            var createMigrationTableScript = "SET ANSI_NULLS ON" +
                                              "  SET QUOTED_IDENTIFIER ON" +
                                              " SET ANSI_PADDING ON" +
                                              "  IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Migration]') AND type in (N'U'))" +
                                              "  BEGIN" +
                                              "  CREATE TABLE [dbo].[Migration](" +
                                              "      [Id] [uniqueidentifier] NOT NULL," +
                                              "      [Script] [varchar](250) NOT NULL," +
                                              "      [ExecutedOn] [datetime] NOT NULL," +
                                              "   CONSTRAINT [PK_Migration] PRIMARY KEY CLUSTERED " +
                                              "  (" +
                                              "      [Id] ASC" +
                                              "  )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]" +
                                              "  ) ON [PRIMARY]" +
                                              "  END" +
                                              "  SET ANSI_PADDING OFF ";

            Context.ExecuteNonQuery(createMigrationTableScript, new Dictionary<string,object> ());
            //Get all the scripts in the Migrations Folder
            var di = new DirectoryInfo(Path.Combine(path,"Migrations"));
            //check if the folder exists and if there is any file inside of it
            if (di.Exists && di.GetFiles("*.sql").Length > 0)
            {
                var scripts = di.GetFiles("*.sql").ToList();
                //get all the scripts that have been executed
                var executed = Context.GetAll();
                //check if the initial script has been executed
                var initialized = executed.FirstOrDefault(s => s.Script == "initializedatabase.sql") != null;
                //compare both lists
                foreach (var e in executed)
                {
                    var s = scripts.FirstOrDefault(sc => sc.Name.ToLower() == e.Script);
                    if (s != null)
                        scripts.Remove(s);
                }
                //check if there is any script pending to run
                if (scripts.Count > 0)
                { 
                    
                    //check if the initializedatabase script is not on the list 
                    var initScript = scripts.FirstOrDefault(s => s.Name.ToLower() == "initializedatabase.sql");
                    //if the init script is there and the DB has not been initialized run it and record it
                    if (initScript != null && !initialized)
                    {
                        RunScript(initScript);
                        scripts.Remove(initScript);
                    }
                    else if (initScript != null)//remove it from the list, the init script can only be run once
                        scripts.Remove(initScript);
                    else if (initScript == null && !initialized) //if the init script is not there and the database has not been initialized,                         
                        Context.Insert(new Migration { Id = Guid.NewGuid(), Script = "initializedatabase.sql", ExecutedOn = DateTime.Now }); //asume that we will not provive and init script, so the current state of the DB is the initial state and we will just be tracking patches

                    //order the scripts by number
                    var scriptList = new SortedList<int, FileInfo>();
                    foreach (var s in scripts)
                    {
                        var orderS = s.Name.ToLower().Replace("patch_", "").Replace(".sql", "");
                        int order;
                        if (int.TryParse(orderS, out order))
                        {
                            scriptList.Add(order, s);
                        }
                    }
                    if (executed.Count() == 0)
                        return;
                    //Get the latest script ran
                    var latest = executed.OrderBy(e => e.ExecutedOn).Last();
                    int latestExecuted = !latest.Script.Contains("patch") ? 0 : int.Parse(latest.Script.Replace("patch_", "").Replace(".sql", ""));


                    //run the rest of the scripts
                    foreach (var s in scriptList.Keys)
                    {
                        if (s > latestExecuted)
                        {
                            RunScript(scriptList[s]);
                        }
                        
                    }
                    
                }
            }
        }

        private static void RunScript(FileInfo script)
        { 
            //get the content
            var scriptContent = script.OpenText().ReadToEnd();
            SqlConnection connection = new SqlConnection(_ConnectionString);
            Server server = new Server(new ServerConnection(connection));
            //execute it
            server.ConnectionContext.ExecuteNonQuery(scriptContent);
            //insert a row in the Migrations table 
            Context.Insert(new Migration { Id = Guid.NewGuid(), Script = script.Name.ToLower(), ExecutedOn = DateTime.Now });
        }

    }
}
