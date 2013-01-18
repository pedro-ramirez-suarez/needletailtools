<h1><a href="http://download-codeplex.sec.s-msft.com/Download?ProjectName=needletailtools&amp;DownloadId=523870"><img style="padding-left: 0px; padding-right: 0px; display: inline; padding-top: 0px; border: 0;" title="logo" src="https://raw.github.com/pedro-ramirez-suarez/needletailtools/master/logo.png" border="0" alt="logo" width="73" height="56" /></a> Needletail Tools</h1>
<p>Different tools that help to build mostly web applications(Asp.Net and Asp.Net MVC), some tools can also be used for windows development.</p>
<h3>Pre requisites</h3>
<p>To build the source code you will need to add references to the following dlls:</p>
<ul>
<li>Microsoft.SqlServer.Types </li>
<li>Microsoft.SqlServer.ConnectionInfo.dll </li>
<li>Microsoft.SqlServer.Management.Sdk.Sfc.dll </li>
<li>Microsoft.SqlServer.Smo.dll </li>
<li>System.Data.SqlServerCe.dll </li>
</ul>
<h3>DataAccess</h3>
<p>A Micro ORM that is fast and easy to use, supports different DBMSs, for now MSSQL and SQLServer CE are fully supported, there is also a version for MySQL that is being tested, read more <a href="https://github.com/pedro-ramirez-suarez/needletailtools/wiki/Using-Needletail#wiki-dataaccess">here</a>.</p>
<h4>Usage for DataAccess</h4>
<p>Ideal when  you need fast access to the database or when you need to change DBMS whitout any hassle, for instance having SQLCE for dev and MSSQL for production.</p>

<h3>DataAccess.Migrations</h3>
<p>A tool to manage database migrations as part of your application, you can use this tool to initialize migrate and seed the database, very simple to use and understand, read more <a href="https://github.com/pedro-ramirez-suarez/needletailtools/wiki/Using-Needletail#wiki-dataaccessmigrations"> here</a>.</p>
<h4>Usage for Migrations</h4>
<p>A good solution for applications running under shared hosting or when you need to manage Database migrations as part of your application.</p>

<h3>Needletail.Mvc</h3>
<p>Needletail.Mvc allows you to call javascript code anywhere from your MVC project, 
this is allows you to tell the client(browser) to call some function when an event occurs on the server's code in real time.
</p>
<p>
Only works with MVC 4 and with browsers that support SSE(sorry, no IE).
</p>
<h4>Usage for Needletail.Mvc</h4>
<p>When you need to execute code on the browser when something happens on the server, the most basic scenario for this, is a chat application</p>