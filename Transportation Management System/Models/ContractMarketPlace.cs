﻿/* -- FILEHEADER COMMENT --
    FILE		:	ContractMarketPlace.cs
    PROJECT		:	Transportation Management System
    PROGRAMMER	:  * Ana De Oliveira
                   * Icaro Ryan Oliveira Souza
                   * Lazir Pascual
                   * Rohullah Noory
    DATE		:	2021-12-07
    DESCRIPTION	:	This file contains the source for the ContractMarketPlace class.
*/

using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace Transportation_Management_System
{
    ///
    /// \class ContractMarketPlace
    ///
    /// \brief The purpose of this class is to manage all the communication with the Contract Market Place.
    ///
    /// \author <i>Team Blank</i>
    ///
    public class ContractMarketPlace
    {
        /// The ContractMarketPlace database IP
        public string CMPServer { get; set; }

        /// The name of the ContractMarketPlace database
        public string DBName { get; set; }

        /// The username to connect to the ContractMarketPlace database
        public string UID { get; set; }

        /// The password to connect to the database
        public string Password { get; set; }

        /// The port to connect to the ContractMarketPlace database
        public int Port { get; set; }

        ///
        /// \brief Construct the ContractMarketPlace object by setting the database connection information.
        ///
        public ContractMarketPlace()
        {
            CMPServer = "159.89.117.198";
            Port = 3306;
            DBName = "cmp";
            UID = "DevOSHT";
            Password = "Snodgr4ss!";
        }

        ///
        /// \brief Returns the string connection for the database.
        ///
        /// \return String representation of the database connection info.
        ///
        public override string ToString()
        {
            return $"SERVER={CMPServer};DATABASE={DBName};PORT={Port};UID={UID};PASSWORD={Password}";
        }

        ///
        /// \brief Returns a list of all contracts fetched from the ContractMarketPlace
        ///
        /// \return A list of all active contracts from the ContractMarketPlace
        ///
        public List<Contract> GetContracts()
        {
            List<Contract> contracts = new List<Contract>();
            try
            {
                string conString = ToString();
                using (MySqlConnection con = new MySqlConnection(conString))
                {
                    MySqlCommand cmd = new MySqlCommand("SELECT * FROM Contract", con);
                    con.Open();
                    MySqlDataReader rdr = cmd.ExecuteReader();

                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            Contract cons = new Contract
                            {
                                ClientName = rdr["CLIENT_NAME"].ToString(),
                                JobType = (JobType)int.Parse(rdr["JOB_TYPE"].ToString()),
                                Quantity = int.Parse(rdr["QUANTITY"].ToString()),
                                Origin = (City)Enum.Parse(typeof(City), rdr["ORIGIN"].ToString(), true),
                                Destination = (City)Enum.Parse(typeof(City), rdr["DESTINATION"].ToString(), true),
                                VanType = (VanType)int.Parse(rdr["VAN_TYPE"].ToString())
                            };
                            contracts.Add(cons);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return contracts;
        }
    }
}