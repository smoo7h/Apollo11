using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using Apollo11.Module.BusinessObjects;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using DevExpress.ExpressApp.Model;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.ExpressApp.Win.Editors;
using Apollo11.Module.BusinessObjects.Trade;

namespace Apollo11.Module.Win.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class CurrentSellVC : ViewController
    {
        public CurrentSellVC()
        {
            InitializeComponent();
            TargetViewType = ViewType.ListView;
            TargetObjectType = typeof(Order);
            TargetViewNesting = Nesting.Nested;

            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();


            
            // Perform various tasks depending on the target View.
        }


        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();

                //start refresh timer
     //      startTimer();
          
           


         //   SortingHelper.Sort(this.View.ObjectSpace)
            // Access and customize the target View control.
        }

        private void SetSelectedContainer()
        {

            
            GridListEditor listEditor = ((ListView)View).Editor as GridListEditor;
            if (listEditor != null)
            {
                //var lastHandle = listEditor.GridView.DataRowCount - 1;
                var lastHandle = 0;
                listEditor.GridView.RefreshData();
                listEditor.GridView.SelectRow(lastHandle);
                
            }
        }


        public System.Windows.Forms.Timer t;
        public void startTimer()
        {
            t = new System.Windows.Forms.Timer();
            t.Tick += T_Tick;
            t.Interval = 700;
            t.Enabled = true;
            t.Start();

        }

        private void T_Tick(object sender, EventArgs e)
        {
            this.View.ObjectSpace.Refresh();
            SetSelectedContainer();
            //this.View.CurrentObject = null;

            //   this.View.Refresh();
        }

      

        protected override void OnDeactivated()
        {
            if (t != null)
            {
                t.Stop();
                t.Dispose();
            }
           
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        private void simpleAction1_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if (t != null)
            {
                t.Stop();
                t.Dispose();
            }
            else
            {
                startTimer();
            }
        }
    }

    public class SortingHelper
    {
        public static void Sort(XPBaseCollection collection, string property, SortingDirection direction)
        {
            bool isSortingAdded = false;
            foreach (SortProperty sortProperty in collection.Sorting)
            {
                if (sortProperty.Property.Equals(DevExpress.Data.Filtering.CriteriaOperator.Parse(property)))
                {
                    isSortingAdded = true;
                }
            }
            if (!isSortingAdded)
            {
                collection.Sorting.Add(new SortProperty(property, direction));
            }
        }
    }
}
