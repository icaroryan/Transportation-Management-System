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
    /// Interaction logic for CarrierSelection.xaml
    /// </summary>
    public partial class CarrierSelection : Window
    {
        Order currentOrder;
        public CarrierSelection()
        {
            InitializeComponent();
        }

        public CarrierSelection(List<CarrierCity> carriers, Order order)
        {
            InitializeComponent();
            currentOrder = order;
            if (order.JobType == JobType.FTL)
            {
                CreateCarrierFTL(carriers);
            }
            else
            {
                /// Update the remaining quantity
                CreateCarrierLTL(carriers);
            }
        }

        private void SelectCarrier_Click(object sender, RoutedEventArgs e)
        {
            if (CarrierList.SelectedItem == null)
            {
                MessageBox.Show("Please select a carrier!", "Invalid Carrier", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                Planner planner = new Planner();

                // Add carrier to order
                var currentCarrier = CarrierList.SelectedItem;
                if (currentCarrier is FTL)
                {
                    FTL FTLCarrier = (FTL)CarrierList.SelectedItem;
                    planner.SelectOrderCarrier(currentOrder, FTLCarrier.CarrierID);

                }
                else
                {
                    LTL LTLCarrier = (LTL)CarrierList.SelectedItem;
                    planner.SelectOrderCarrier(currentOrder, LTLCarrier.CarrierID);


                    // If current carrier does not have enough availability for the order, select another carrier to fullfill the rest
                    if (LTLCarrier.LTLAval < currentOrder.Quantity)
                    {
                        // Remove from list if the current carrier doesn't have enough availabity
                        CarrierList.Items.Remove(currentCarrier);
                        currentOrder.Quantity -= LTLCarrier.LTLAval;
                    }
                    // If we have more than 26 pallets, we need to have more than 1 trip
                    else if (currentOrder.Quantity > 26)
                    {
                        currentOrder.Quantity -= 26;

                        // Update the remaining quantity of the carrier
                        ((LTL)CarrierList.SelectedItem).LTLAval -= 26;

                        // Refresh Items
                        CarrierList.Items.Refresh();
                    }
                    else
                    {
                        currentOrder.Quantity -= LTLCarrier.LTLAval;
                    }
                    
                    
                    // Update remaining quantity if still have left
                    if (currentOrder.Quantity > 0)
                    {
                        // Update the remaining quantity
                        OrderQuantity.Content = $"Remaining Quantity from Order: {currentOrder.Quantity}";                      
                    }
                }
                

                // If multiple trips were selected if needed
                if (currentOrder.Quantity <= 0)
                {
                    DialogResult = true;
                    Close();
                }
                
            }     
        }

        private class LTL
        {
            public int CarrierID { get; set; }
            public string Name { get; set; }
            public City DepotCity { get; set; }
            public int LTLAval { get; set; }
            public double LTLRate { get; set; }
        }

        private class FTL
        {
            public int CarrierID { get; set; }
            public string Name { get; set; }
            public City DepotCity { get; set; }
            public int FTLAval { get; set; }
            public double FTLRate { get; set; }
            public double ReeferCharge { get; set; }
        }

        private void CreateCarrierLTL(List<CarrierCity> carriers)
        {
            var gridView = new GridView();
            CarrierList.View = gridView;
            gridView.Columns.Add(new GridViewColumn
            {
                Header = "Carrier Name",
                DisplayMemberBinding = new Binding("Name")
            });
            gridView.Columns.Add(new GridViewColumn
            {
                Header = "Depot City",
                DisplayMemberBinding = new Binding("DepotCity")
            });
            gridView.Columns.Add(new GridViewColumn
            {
                Header = "LTL Availability",
                DisplayMemberBinding = new Binding("LTLAval")
            });
            gridView.Columns.Add(new GridViewColumn
            {
                Header = "LTL Rate",
                DisplayMemberBinding = new Binding("LTLRate")
            });

            foreach (var carrier in carriers)
            {
                CarrierList.Items.Add(new LTL{ CarrierID = (int)carrier.Carrier.CarrierID, Name = carrier.Carrier.Name, DepotCity = carrier.DepotCity, LTLAval = carrier.LTLAval, LTLRate = carrier.Carrier.LTLRate });
            }
        }

        private void CreateCarrierFTL(List<CarrierCity> carriers)
        {
            var gridView = new GridView();
            CarrierList.View = gridView;
            gridView.Columns.Add(new GridViewColumn
            {
                Header = "Carrier Name",
                DisplayMemberBinding = new Binding("Name")
            });
            gridView.Columns.Add(new GridViewColumn
            {
                Header = "Depot City",
                DisplayMemberBinding = new Binding("DepotCity")
            });
            gridView.Columns.Add(new GridViewColumn
            {
                Header = "FTL Availability",
                DisplayMemberBinding = new Binding("FTLAval")
            });
            gridView.Columns.Add(new GridViewColumn
            {
                Header = "FTL Rate",
                DisplayMemberBinding = new Binding("FTLRate")
            });
            gridView.Columns.Add(new GridViewColumn
            {
                Header = "Reefer Charge",
                DisplayMemberBinding = new Binding("ReeferCharge")
            });

            foreach (var carrier in carriers)
            {
                CarrierList.Items.Add(new FTL { CarrierID = (int)carrier.Carrier.CarrierID, Name = carrier.Carrier.Name, DepotCity = carrier.DepotCity, FTLAval = carrier.FTLAval, FTLRate = carrier.Carrier.FTLRate, ReeferCharge = carrier.Carrier.ReeferCharge });
            }
        }
    }
}
