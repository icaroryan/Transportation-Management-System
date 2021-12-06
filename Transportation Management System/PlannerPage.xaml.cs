﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Transportation_Management_System
{
    /// <summary>
    /// Interaction logic for PlannerPage.xaml
    /// </summary>
    public partial class PlannerPage : Window
    {
        private Planner planner = new Planner();

        public PlannerPage()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            App.Current.MainWindow.Visibility = Visibility.Visible;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Orders_Click(sender, e);
        }

        private void Invoices_Click(object sender, RoutedEventArgs e)
        {
            ResetStatus();
            ReportsGrid.Visibility = Visibility.Visible;
            Invoices.Background = Brushes.LightSkyBlue;

            AllInvoices.IsChecked = true;

            Refresh_Invoices();
        }

        private void Activities_Click(object sender, RoutedEventArgs e)
        {
            ResetStatus();
            ActivitiesGrid.Visibility = Visibility.Visible;
            Activities.Background = Brushes.LightSkyBlue;
        }


        private void Orders_Click(object sender, RoutedEventArgs e)
        {
            ResetStatus();            

            AllBox.IsChecked = true;

            Orders.Background = Brushes.LightSkyBlue;
            OrdersGrid.Visibility = Visibility.Visible;

            Refresh_Orders();
        }

        

        private void AllInvoices_Click(object sender, RoutedEventArgs e)
        {
            Refresh_Invoices();
        }

        private void PastInvoices_Click(object sender, RoutedEventArgs e)
        {
            Refresh_Invoices();
        }


        private void Report_Click(object sender, RoutedEventArgs e)
        {
            ResetStatus();
            ReportsGrid.Visibility = Visibility.Visible;
            GenerateReport.Background = Brushes.LightSkyBlue;
            AllInvoices.IsChecked = true;

            Refresh_Invoices();
        }

        private void Refresh_Invoices()
        {
            List<Invoice> invoicesList = new List<Invoice>();

            if(AllInvoices.IsChecked == true)
            {
                // past invoice is also checked, get all invoices
                invoicesList = planner.GenerateSummaryReport(true);           
               
            }
            else if (PastInvoice.IsChecked == true)
            {
                // completed box is checked, fetch only completed orders
                invoicesList = planner.GenerateSummaryReport(false);
            }

            ReportList.ItemsSource = invoicesList;           
        }           
        

        private void Refresh_Orders()
        {
            var orderList = new List<Order>();

            if(AllBox.IsChecked == true)
            {
                // Get all orders
                orderList = planner.FetchOrders(2);
            }
            else if (ActiveBox.IsChecked == true)
            {
                // Get active orders
                orderList = planner.FetchOrders(0);
            }
            else if(CompletedBox.IsChecked == true)
            {
                // completed box is checked, fetch only completed orders
                orderList = planner.FetchOrders(1);
                
                ViewCarrier.Visibility = Visibility.Hidden;
                CompleteOrder.Visibility = Visibility.Hidden;
                OrderProgress.Visibility = Visibility.Hidden;
            }

            OrdersList.ItemsSource = orderList;
        }

        private void AllBox_Click(object sender, RoutedEventArgs e)
        {
            Refresh_Orders();
        }

        private void ActiveBox_Click(object sender, RoutedEventArgs e)
        {
            Refresh_Orders();
        }

        private void CompletedBox_Click(object sender, RoutedEventArgs e)
        {
            Refresh_Orders();
        }

        private void View_Carrier(object sender, RoutedEventArgs e)
        {
            Order currentOrder = (Order) OrdersList.SelectedItem;
            List<CarrierCity> carrierCity = planner.GetCarriers(currentOrder.Origin.ToString(), currentOrder.JobType);
            CarrierSelection selectCarrier = new CarrierSelection(carrierCity, currentOrder);
            if (selectCarrier.ShowDialog() == true)
            {
                CompleteOrder.Visibility = Visibility.Visible;
                OrderProgress.Visibility = Visibility.Visible;
                ViewCarrier.Visibility = Visibility.Hidden;
            }
        }

        private void Complete_Order(object sender, RoutedEventArgs e)
        {
            Order currentOrder = (Order)OrdersList.SelectedItem;
            planner.CompleteOrder(currentOrder);
            CompleteOrder.Visibility = Visibility.Hidden;
            OrderProgress.Visibility = Visibility.Hidden;
            Refresh_Orders();
        }

        private void Simulate_OrderStatus()
        {
            Order currentOrder = (Order)OrdersList.SelectedItem;

            DateTime expectedDeliveryDate = currentOrder.OrderCreationDate.AddHours(planner.GetTotalTime(currentOrder));
            TimeSpan TotalTimeSpan = expectedDeliveryDate - currentOrder.OrderCreationDate;
            TimeSpan TimePassed = DateTime.Now - currentOrder.OrderCreationDate;

            double result = TimePassed.TotalHours * 100 / TotalTimeSpan.TotalHours;
            if(result > 100)
            {
                result = 100;
            }
            OrderProgressBar.Value = result;
        }

        private void Selection_Changed(object sender, RoutedEventArgs e)
        {
            // order selection has changed
            Order currentOrder = (Order)OrdersList.SelectedItem;
            if (currentOrder != null)
            {
                if (planner.CarrierAssigned(currentOrder) == true && currentOrder.IsCompleted == 0)
                {
                    // carrier has already been assigned, display complete order button
                    Simulate_OrderStatus();
                    CompleteOrder.Visibility = Visibility.Visible;
                    OrderProgress.Visibility = Visibility.Visible;
                    ViewCarrier.Visibility = Visibility.Hidden;
                }
                else
                {
                    // carrier has not been assigned, display view carrier button
                    ViewCarrier.Visibility = Visibility.Visible;
                    CompleteOrder.Visibility = Visibility.Hidden;
                    OrderProgress.Visibility = Visibility.Hidden;
                }

                if (currentOrder.IsCompleted == 1)
                {
                    // current order is completed, hide both buttons
                    ViewCarrier.Visibility = Visibility.Hidden;
                    CompleteOrder.Visibility = Visibility.Hidden;
                    OrderProgress.Visibility = Visibility.Hidden;
                }              
            }                
        }

        private void ResetStatus()
        {
            // Reset all buttons background
            Invoices.Background = Brushes.WhiteSmoke;
            Activities.Background = Brushes.WhiteSmoke;
            Orders.Background = Brushes.WhiteSmoke;
            GenerateReport.Background = Brushes.WhiteSmoke;

            // Hide all grids
            ReportsGrid.Visibility = Visibility.Hidden;
            ActivitiesGrid.Visibility = Visibility.Hidden;
            OrdersGrid.Visibility = Visibility.Hidden;

            ActiveBox.IsChecked = false;
            CompletedBox.IsChecked = false;
            AllInvoices.IsChecked = false;
            PastInvoice.IsChecked = false;
        }

    }
}
