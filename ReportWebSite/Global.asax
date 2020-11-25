<%@ Application Language="C#" %>

<script runat="server">

	const string outdir = @"c:\inetpub\wwwroot\gukv\Log\";

    void Application_Start(object sender, EventArgs e) 
    {
        Utils.EnsureRequestValidationMode();
        log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(Server.MapPath("~/Web.config"))); 
        DevExpress.Web.ASPxWebControl.BackwardCompatibility.DataControlAllowReadUnlistedFieldsFromClientApiDefaultValue = true;
    }

    void Application_PreRequestHandlerExecute(object sender, EventArgs e)
    {
        DevExpress.Web.ASPxWebControl.GlobalTheme = Utils.CurrentTheme;
    }
    
    void Application_BeginRequest()
    {
		var request = Request.QueryString;
		var rawUrl = Request.RawUrl;
        //System.IO.File.AppendAllText(@"C:\inetpub\wwwroot\gukv\Test\log.txt", "Application_BeginRequest: rawUrl=" + rawUrl + "\n");
		if (rawUrl.ToLower().StartsWith(@"/ImgContent/".ToLower()) || rawUrl.ToLower().StartsWith(@"/gukv/ImgContent/".ToLower()))
		{
			rawUrl = rawUrl.ToLower().Replace(@"/ImgContent/".ToLower(), "/Reports1NF/ImgContent.aspx?photofilename=");
            //System.IO.File.AppendAllText(@"C:\inetpub\wwwroot\gukv\Test\log.txt", "Application_BeginRequest: rawUrl2=" + rawUrl + "\n");
			Response.Redirect(rawUrl);
		}


        //if (Request.IsLocal) { StackExchange.Profiling.MiniProfiler.Start(); }
  
		//var response = HttpContext.Current.Response;

		//var filter = new OutputFilterStream(response.Filter);
		//filter.Id = Guid.NewGuid();
		//response.Filter = filter;
		//Request.SaveAs(outdir + filter.Id + ".req", true);
//                    Request.SaveAs(outdir + Guid.NewGuid() + ".req", true);

   }

    void Application_EndRequest()
    {
 
		if (Response.Filter is OutputFilterStream)
		{
//			var filter = (OutputFilterStream)(Response.Filter);
//			var outbytes = filter.ReadBytes();
//			System.IO.File.WriteAllBytes(outdir + filter.Id + ".out", outbytes);
		}

       StackExchange.Profiling.MiniProfiler.Stop();
    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  Code that runs on application shutdown

    }
        
    void Application_Error(object sender, EventArgs e) 
    { 
        // Code that runs when an unhandled error occurs

	var ex = Server.GetLastError();
	var lognet = log4net.LogManager.GetLogger("ReportWebSite");
	lognet.Debug("--------------- Application_Error ----------------", ex);

    }

    void Session_Start(object sender, EventArgs e) 
    {
        // Code that runs when a new session is started

    }

    void Session_End(object sender, EventArgs e) 
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }
    
   
</script>
