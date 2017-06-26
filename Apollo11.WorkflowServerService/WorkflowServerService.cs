using System;
using System.Configuration;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using DevExpress.ExpressApp.Workflow.Server;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Workflow;
using DevExpress.Workflow;
using DevExpress.ExpressApp.MiddleTier;
using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Drawing.Imaging;
using System.Threading;

namespace Apollo11.WorkflowServerService {
    public partial class Apollo11WorkflowServer  : System.ServiceProcess.ServiceBase {
        private WorkflowServer server;
        protected override void OnStart(string[] args) {

            IObjectSpace newSpace = null;
            if(server == null) {
                ServerApplication serverApplication = new ServerApplication();
                serverApplication.ApplicationName = "Apollo11";
				serverApplication.CheckCompatibilityType = CheckCompatibilityType.DatabaseSchema;
                // The service can only manage workflows for those business classes that are contained in Modules specified by the serverApplication.Modules collection.
                // So, do not forget to add the required Modules to this collection via the serverApplication.Modules.Add method.
                serverApplication.Modules.BeginInit();
                serverApplication.Modules.Add(new WorkflowModule());
                serverApplication.Modules.Add(new Apollo11.Module.Apollo11Module());
                serverApplication.Modules.Add(new Apollo11.Module.Win.Apollo11WindowsFormsModule());
                serverApplication.Modules.Add(new Apollo11.Module.Web.Apollo11AspNetModule());
                if(ConfigurationManager.ConnectionStrings["ConnectionString"] != null) {
                    serverApplication.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                }
                serverApplication.Setup();
                serverApplication.Logon();

                IObjectSpaceProvider objectSpaceProvider = serverApplication.ObjectSpaceProvider;

                WorkflowCreationKnownTypesProvider.AddKnownType(typeof(DevExpress.Xpo.Helpers.IdList));

                server = new WorkflowServer("http://localhost:46232", objectSpaceProvider, objectSpaceProvider);
                server.StartWorkflowListenerService.DelayPeriod = TimeSpan.FromSeconds(15);
                server.StartWorkflowByRequestService.DelayPeriod = TimeSpan.FromSeconds(15);
                server.RefreshWorkflowDefinitionsService.DelayPeriod = TimeSpan.FromMinutes(15);

                server.CustomizeHost += delegate(object sender, CustomizeHostEventArgs e) {
                    e.WorkflowInstanceStoreBehavior.WorkflowInstanceStore.RunnableInstancesDetectionPeriod = TimeSpan.FromSeconds(15);
                };

                server.CustomHandleException += delegate(object sender, CustomHandleServiceExceptionEventArgs e) {
                    Tracing.Tracer.LogError(e.Exception);
                    e.Handled = false;
                };


              

                //init APIS

                BitfinexRestAPI bitapi = new BitfinexRestAPI();
                Quadrigacx quaAPI = new Quadrigacx(objectSpaceProvider.CreateObjectSpace());


                Arguements package = new Arguements();
                package.BitAPI = bitapi;
                package.QuadAPI = quaAPI;

                package.Space = objectSpaceProvider.CreateObjectSpace();

                //    bitapi.CreateTrade(Convert.ToDecimal(1186.1),Convert.ToDecimal(0.016),TradeType.Buy);

                //    Console.WriteLine("Created trade " + DateTime.Now.ToString());
                //   Console.ReadLine();



                //
                //start logging live quad data from html site
                BackgroundWorker qwrBW = new BackgroundWorker();

                qwrBW.DoWork += Qwr_DoWork;
                qwrBW.RunWorkerCompleted += QwrBW_RunWorkerCompleted;
                qwrBW.ProgressChanged += QwrBW_ProgressChanged;

           

          //      qwrBW.RunWorkerAsync(objectSpaceProvider.CreateObjectSpace());
                qwrBW.RunWorkerAsync(objectSpaceProvider.CreateObjectSpace());

                //
                //statrt logging bitstamp data

                //BackgroundWorker bWBitStamp = new BackgroundWorker();
                //bWBitStamp.DoWork += QwrBitStamp_DoWork;
                //bWBitStamp.RunWorkerCompleted += QwrBitStamp_RunWorkerCompleted;
                //bWBitStamp.ProgressChanged += QwrBitStamp_ProgressChanged;

                //bWBitStamp.RunWorkerAsync(objectSpaceProvider.CreateObjectSpace());
                //    StartLiveBitStampScraper(objectSpaceProvider.CreateObjectSpace());



                //
                //start logging Bitfinex orders
                BackgroundWorker bwBitFinex = new BackgroundWorker();
                bwBitFinex.DoWork += BwBitFinex_DoWork;
                bwBitFinex.RunWorkerCompleted += BwBitFinex_RunWorkerCompleted;
                bwBitFinex.ProgressChanged += BwBitFinex_ProgressChanged;

                bwBitFinex.RunWorkerAsync(package);



                //create trade
                //       Quadrigacx API = new Quadrigacx(objectSpaceProvider.CreateObjectSpace());




                //  Console.WriteLine(DateTime.Now.ToLongTimeString() + " creating trade");
                //    API.CreateSellTrade(Convert.ToDecimal( 0.005), Convert.ToDecimal(1130.98));

                //API.CreateBuyTrade(Convert.ToDecimal(0.005), Convert.ToDecimal(1110.98));

                //var orders = API.QuadrigaAPIClient.GetOpenOrders("btc_cad");
                //  Console.WriteLine(orders.Count().ToString());





            }
            server.Start();
            LogMessage(DateTime.Now + " Server Started ...");


         


        }

       

        private void BwBitFinex_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
          //  throw new NotImplementedException();
        }

        private void BwBitFinex_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void BwBitFinex_DoWork(object sender, DoWorkEventArgs e)
        {
            //  BitfinexWebsocketAPI bitfinx = new BitfinexWebsocketAPI(e.Argument as IObjectSpace);

            //   BitfinexWebsocketAPI.GetData(e.Argument as IObjectSpace);
            Arguements a = e.Argument as Arguements;

            BitFinexLiveScraper bs = new BitFinexLiveScraper(e.Argument as IObjectSpace);
            bs.BitAPi = a.BitAPI;
            bs.QuaAPI = a.QuadAPI;
            bs.GetData();

        }

        private void StampAPIBW_DoWork(object sender, DoWorkEventArgs e)
        {
            //   BitfinexWebsocketAPI bitfinex = new BitfinexWebsocketAPI(e.Argument as IObjectSpace);

            Bitstamp bitstamp = new Bitstamp(e.Argument as IObjectSpace, "");



            bitstamp.GetData();









        }

        IObjectSpace Space;

        private void StampAPIBW_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void StampAPIBW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {





        }

        private void QwrBitStamp_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void QwrBitStamp_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void QwrBitStamp_DoWork(object sender, DoWorkEventArgs e)
        {
           StartLiveBitStampScraper(e.Argument as IObjectSpace);
        }

        private void QwrBW_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
          //  throw new NotImplementedException();
        }

        private void QwrBW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
           // throw new NotImplementedException();
        }

        private void Qwr_DoWork(object sender, DoWorkEventArgs e)
        {

            StartLiveQuadScraper(e.Argument as IObjectSpace);
        }

        protected override void OnStop() {
            server.Stop();
        }

        public void StartLiveQuadScraper(IObjectSpace space)
        {
            
            QuadrigacxLiveScraper scraper = new QuadrigacxLiveScraper(space);
            scraper.GetData();
        }

        public void StartLiveBitStampScraper(IObjectSpace space)
        {

            BitStampLiveScraper scraper = new BitStampLiveScraper(space);
            
            scraper.GetData();
        }

        public void Start()
        {
            //this method is used to run the console application
            LogMessage(DateTime.Now + " Server Starting ...");
            OnStart(new string[0]);



        }

        public Apollo11WorkflowServer() {
            InitializeComponent();
        }

        private static void LogError(string error, IRunningWorkflowInstanceInfo usefulInfo/*gabe's*/)
        {
            using (StreamWriter sw = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\GibsWorkFlowServerErrorFile.txt", true))
            {
                Console.WriteLine(String.Format("{0}  {1}", DateTime.Now, error));
                sw.Write(String.Format("{0}  {1}", DateTime.Now, error));
                sw.Write(Environment.NewLine);
                sw.Write(string.Format("> Workflow ID: {0}\r\n> Instance ID: {1}\r\n--------------------------", usefulInfo.WorkflowUniqueId, usefulInfo.ActivityInstanceId));//gabe's
                sw.Flush();
                sw.Close();
            }
        }

        private static void LogMessage(string message)
        {
            using (StreamWriter sw = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\GibsWorkFlowServerLogFile.txt", true))
            {
                Console.WriteLine(String.Format("{0}  {1}", DateTime.Now, message));
                sw.Write(String.Format("{0}  {1}", DateTime.Now, message));
                sw.Write(Environment.NewLine);
                sw.Flush();
                sw.Close();
            }
        }

    }
    public class Arguements
    {
        public Quadrigacx QuadAPI { get; set; }
        public BitfinexRestAPI BitAPI { get; set; }
        public IObjectSpace Space { get; set; }
    }
}
