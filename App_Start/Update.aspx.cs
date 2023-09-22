using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class Update : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            GridView1.DataKeyNames = new string[] { "Branch_ID" };
            if (!IsPostBack)
            {
                // Get the search text from the session, if any
                searchText = (string)Session["SearchText"];
                BindGridView();
            }
        }

        string searchText;

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            searchText = txtSearch.Text.Trim();
            Session["SearchText"] = searchText;
            BindGridView();
        }

        private void BindGridView()
        {
            String connectionString = ConfigurationManager.ConnectionStrings["HotelManagementSystemConnectionString"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Hotel";

                if (!string.IsNullOrEmpty(searchText))
                {
                    query += " WHERE Location LIKE '%' + @searchKeywords + '%'";
                }

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (!string.IsNullOrEmpty(searchText))
                    {
                        command.Parameters.AddWithValue("@searchKeywords", searchText);
                    }

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        // Filter the results based on the search criteria
                        if (!string.IsNullOrEmpty(searchText))
                        {
                            dt.DefaultView.RowFilter = "Location LIKE '%" + searchText + "%'";
                        }

                        GridView1.DataSource = dt;
                        GridView1.DataBind();

                        if (dt.Rows.Count == 0)
                        {
                            lblError.Visible = true;
                            lblError.ForeColor = Color.Red;
                            lblError.Text = "No results found.";
                        }
                        else
                        {
                            lblError.Visible = false;
                        }
                    }
                }
            }
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            // Get the ID of the row being updated
            int rowID = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values["Branch_ID"]);

            // Get the updated values for the Location and PhoneNumber fields
            string updatedLocation = ((TextBox)GridView1.Rows[e.RowIndex].FindControl("txtLocation")).Text;
            string updatedPhoneNumber = ((TextBox)GridView1.Rows[e.RowIndex].FindControl("txtPhoneNumber")).Text;

            // Update the database with the new values
            String connectionString = ConfigurationManager.ConnectionStrings["HotelManagementSystemConnectionString"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "UPDATE Hotel SET Location = @location, PhoneNumber = @phoneNumber WHERE Branch_ID = @id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@location", updatedLocation);
                    command.Parameters.AddWithValue("@phoneNumber", updatedPhoneNumber);
                    command.Parameters.AddWithValue("@id", rowID);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected == 1)
                    {
                        GridView1.EditIndex = -1;
                        BindGridView();
                    }
                    else
                    {
                        lblError.Visible = true;
                        lblError.ForeColor = Color.Red;
                        lblError.Text = "Error updating record.";
                    }
                }
            }
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            BindGridView();
        }


        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            BindGridView();

            // Reset visibility of all rows
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                GridView1.Rows[i].Visible = true;
            }
        }
    }
}
