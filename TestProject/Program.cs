using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;
using Needletail.DataAccess.Engines;
using DataAccess.Migrations;
using Needletail.TestProject.Model;
using Needletail.DataAccess;
using Needletail.DataAccess.Migrations;

namespace TestProject {
    class Program {


        /*
         First version of Needletail tools.

Don't forget to check the documentation:
[url:https://needletailtools.codeplex.com/documentation]

*This is release is beta, but a few live sites are using this on their production environments *
         
         */
        static void Main(string[] args) {


            //Force Migration on on application startup
            Migrator.Migrate("localConnectionString");
            Console.WriteLine("Done Migrating");
            Console.ReadKey();

            var  context = new DBTableDataSourceBase<Task ,Guid>("localConnectionString", "Tasks"); 

            //Insert a new task
            context.Insert(new Task {  TaskId = Guid.NewGuid(), CreatedOn = DateTime.Now, DueOn = DateTime.Now.AddDays(10), Name= "Sample Task", Description="This is a sample task"});

        }
    }
}
