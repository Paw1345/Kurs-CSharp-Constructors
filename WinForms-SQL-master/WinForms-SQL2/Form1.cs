﻿using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace WinForms_SQL2
{
    public partial class Form1 : Form
    {
   
        private string defaultQuery = "SELECT * FROM CUSTOMERS";

        // Initialize the form.
        public Form1()
        {
            InitializeComponent();
        }


        // On loading Form1
        private void Form1_Load(object sender, EventArgs e)
        {
            // Load data into the 'nORTHWNDDataSet.Customers' table
            this.customersTableAdapter.Fill(this.nORTHWNDDataSet.Customers);
            // Bind the DataGridView to the BindingSource
            customersDataGridView.DataSource = nORTHWNDDataSetBindingSource;
            GetData(defaultQuery);
        }


        // Attempt to fill the table with the provided SQL query string
        // On error - displays messagebox error.message
        private void GetData(string queryStr)
        {
            try
            {
                // Bail if empty string
                if ( String.IsNullOrWhiteSpace(queryStr)) {
                    MessageBox.Show("Please enter an SQL Query", "Error");
                    // Fill textbox with an example query string
                    textBox1.Text = "Example: "+defaultQuery;
                    return;
                }

                // Establish connection string - can be found within the App.config file
                String connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\NORTHWND.MDF;Integrated Security=True;Connect Timeout=30";

                // Create a new data adapter based on the specified query.
                dataAdapter = new SqlDataAdapter(queryStr+";", connectionString);

                // Create a command builder to generate SQL update, insert, and
                // delete commands based on selectCommand. These are used to
                // update the database.     [Currently Not Used.]
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);
                
                // Populate a new data table and bind it to the BindingSource.
                DataTable table = new DataTable();
                table.Locale = System.Globalization.CultureInfo.InvariantCulture;
                dataAdapter.Fill(table);
                nORTHWNDDataSetBindingSource.DataSource = table;

                // Resize the DataGridView columns to fit the newly loaded content.
                customersDataGridView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);
            }
            catch (SqlException SQLe)
            {
                MessageBox.Show(SQLe.Message, "Error");
            }
        }


        private void resetButton_Click(object sender, EventArgs e)
        {
            // Reload all CUSTOMER entries from the database.
            GetData(defaultQuery);
            // And clear the textbox
            textBox1.Clear();
        }


        private void submitButton_Click(object sender, EventArgs e)
        {
            // Update the database with the user's changes.
            dataAdapter.Update((DataTable)nORTHWNDDataSetBindingSource.DataSource);
        }


        private void Button_SubmitQuery_Click(object sender, EventArgs e)
        {
            GetData(textBox1.Text);
        }


        private void Button_About_Click(object sender, EventArgs e)
        {
            MessageBox.Show("proba");
        }
    }
}
