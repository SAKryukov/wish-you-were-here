/*
    FirstTime Utility Test
    
    Copyright (C) 2009-2017 by Sergey A Kryukov
    http://www.SAKryukov.org
*/
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;

namespace FirstTimeWpfSample {
    using FirstTime = SA.Univeral.Utilities.FirstTime;
    
    public partial class WindowOwned : Window {
        
        public WindowOwned() {
            InitializeComponent();
            buttonClose.Click += (s, e) => { Close(); }; 
        } //WindowOwned

        void ReportItem(string item) {
            int itemNumber = listBoxReport.Items.Add(item);
            listBoxReport.ScrollIntoView(listBoxReport.Items[itemNumber]);
        } //ReportItem

        protected override void OnActivated(System.EventArgs e) {
            base.OnActivated(e);
            if (FirstTime.Here)
                ReportItem("FIRST TIME ACTIVATION HERE");
        } //OnActivated

        protected override void OnMouseEnter(MouseEventArgs e) {
            base.OnMouseEnter(e);
            if (FirstTime.Here)
                ReportItem("FIRST TIME Mouse Entered HERE!");
        } //OnMouseEnter

        protected override void OnMouseLeave(MouseEventArgs e) {
            base.OnMouseLeave(e);
            base.OnMouseEnter(e);
            if (FirstTime.Here)
                ReportItem("FIRST TIME Mouse Leaved HERE!");
        } //OnMouseLeave

        FirstTime.Instance FirstTime = new FirstTime.Instance();

    } //class WindowOwned

} //namespace FirstTimeWpfSample
