﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transportation_Management_System
{
    public class Route
    {
        /// The destination city
        public City Destination { set; get; }

        /// The total distance in KM
        public int Distance { set; get; }

        /// The time it takes from one citu to another
        public double Time { set; get; }

        /// The city that is located to the West of destination
        public City West { set; get; }

        /// "Pointer" to the city on west
        public Route WestPtr { set; get; }

        /// The city that is located to the East of destination
        public City East { set; get; }

        // "Pointer" to the city on East
        public Route EastPtr { set; get; }


        public Route() { }

        public Route(City newDestination, int newDistance, double newTime, City newWest, City newEast)
        {
            Destination = newDestination;
            Distance = newDistance;
            Time = newTime;
            West = newWest;
            East = newEast;
        }



        ///
        /// \brief Used to calculate the total distance and time between two cities based on the routes table
        ///
        /// \param origin  - <b>City</b> - Origin city
        /// \param destination  - <b>City</b> - Destination city
        /// 
        /// 
        /// \return A keyValuePair with the distance and time between those two cities
        /// 
        public KeyValuePair<int, double> CalculateDistanceAndTime(City origin, City destination)
        {
            int totalDistance = 0;
            double totalTime = 0.0;

            DAL db = new DAL();
            List<Route> routes = db.GetRoutes();

            // Create the graph
            foreach (var route in routes)
            {
                // If not found (edges), return null
                route.WestPtr = routes.ElementAtOrDefault((int) route.West);
                route.EastPtr = routes.ElementAtOrDefault((int) route.East);
            }

            // Get the current city
            Route curr = routes[(int) origin];

            // While we're not in the destination city
            do
            {
                //(5.5 Hours)
                // If going east
                if (origin < destination)
                {
                    // Windsor --> London --> Hamilton 

                    curr = curr.EastPtr;
                }
                // If goint west
                else if (destination < origin)
                {
                    // Hamilton --> London --> Windsor

                    curr = curr.WestPtr;
                }
            } while (curr.Destination != destination);


            return new KeyValuePair<int, double>(totalDistance, totalTime);
        }
    }
}
