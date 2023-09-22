using Microsoft.AspNet.FriendlyUrls;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class RoomInformation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            String Hotel = ConfigurationManager.ConnectionStrings["HotelManagementSystemConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(Hotel))
            {
                SqlCommand cmd = new SqlCommand("Select * from Hotel", con);
                con.Open();
                DropDownList1.DataTextField = "Branch_ID";
                DropDownList1.DataSource = cmd.ExecuteReader();
                DropDownList1.DataBind();
                con.Close();

                SqlCommand cmd1 = new SqlCommand("Select * from RoomType", con);
                con.Open();
                DropDownList2.DataTextField = "RoomType_ID";
                DropDownList2.DataSource = cmd1.ExecuteReader();
                DropDownList2.DataBind();
                con.Close();
            }
            
            GridView1.DataKeyNames = new string[] { "Room_ID" };
            if (!IsPostBack)
            {
                // Get the search text from the session, if any
                searchText = "";
                BindGridView(); 
                
            }
        }

        string searchText;
        protected void Add_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            panel2.Visible = true;
            panel3.Visible = false;
            RoomNo.Text = "";
        }

        protected void Update_Delete_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            panel3.Visible = true;
            panel2.Visible = false;
        }

        protected void AddRoom_Click(object sender, EventArgs e)
        {
            String Hotel = ConfigurationManager.ConnectionStrings["HotelManagementSystemConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(Hotel))
            {
                string query = "INSERT INTO Room (RoomNumber,RoomType_ID,Branch_ID)" +
                               "VALUES (@RoomNumber,@RoomType_ID,@Branch_ID)";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@RoomNumber", RoomNo.Text);
                    
                    cmd.Parameters.AddWithValue("@RoomType_ID", DropDownList2.SelectedItem.Value);
                    cmd.Parameters.AddWithValue("@Branch_ID", DropDownList1.SelectedItem.Value);
                 

                    con.Open();
                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        GridView1.DataKeyNames = new string[] { "Room_ID" };
                        // Get the search text from the session, if any
                        searchText = (string)Session["SearchText"];
                        BindGridView();

                        panel1.Visible = true;
                        panel2.Visible = false;

                    }
                    con.Close();

                }
            }

        }

        protected void Search_Click(object sender, EventArgs e)
        {
            searchText = SearchRoom.Text.Trim();
            Session["SearchText"] = searchText;
            BindGridView();
        }

        private void BindGridView()
        {
            String connectionString = ConfigurationManager.ConnectionStrings["HotelManagementSystemConnectionString"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Room";

                if (!string.IsNullOrEmpty(searchText))
                {
                    if (idradio.Checked)
                    {
                        int roomid;
                        if (int.TryParse(searchText, out roomid))
                        {
                            query += " WHERE Room_ID =@searchKeywords";
                            searcherror.Visible = false;
                        }
                        else
                        {
                            searcherror.Visible = true;
                            searcherror.ForeColor = Color.Red;
                            searcherror.Text = "Invalid search input for Room ID";
                            return;
                        }
                    }
                    else if (roomradio.Checked)
                    {
                        int roomnum;
                        if (int.TryParse(searchText, out roomnum))
                        {
                            query += " WHERE RoomNumber =@searchKeywords";
                            searcherror.Visible = false;
                        }
                        else
                        {
                            searcherror.Visible = true;
                            searcherror.ForeColor = Color.Red;
                            searcherror.Text = "Invalid search input for Room Number";
                            return;
                        }
                    }

                    else if (typeradio.Checked)
                    {
                        int roomtypeid;
                        if (int.TryParse(searchText, out roomtypeid))
                        {
                            query += " WHERE RoomType_ID =@searchKeywords";
                            searcherror.Visible = false;
                        }
                        else
                        {
                            searcherror.Visible = true;
                            searcherror.ForeColor = Color.Red;
                            searcherror.Text = "Invalid search input for Room Type ID";
                            return;
                        }
                    }
                    else if (branchradio.Checked)
                    {
                        int branchid;
                        if (int.TryParse(searchText, out branchid))
                        {
                            query += " WHERE Branch_ID =@searchKeywords";
                            searcherror.Visible = false ;
                        }
                        else
                        {
                            searcherror.Visible = true;
                            searcherror.ForeColor = Color.Red;
                            searcherror.Text = "Invalid search input for Branch ID";
                            return;
                        }

                    }
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

                            if (idradio.Checked)
                            {
                                dt.DefaultView.RowFilter = "Room_ID= '" + searchText + "'";
                            }
                            else if (roomradio.Checked)
                            {
                                dt.DefaultView.RowFilter = "RoomNumber= '" + searchText + "'";
                            }
                           
                            else if (typeradio.Checked)
                            {
                                dt.DefaultView.RowFilter = "RoomType_ID= '" + searchText + "'";
                            }
                           
                            else if (branchradio.Checked)
                            {
                                dt.DefaultView.RowFilter = "Branch_ID= '" + searchText + "'";
                            }
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

        protected void DeleteRow(int id)
        {
            String connectionString = ConfigurationManager.ConnectionStrings["HotelManagementSystemConnectionString"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "DELETE FROM Room WHERE Room_ID = @id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    command.ExecuteNonQuery();
                }
            }

            // Refresh the grid view to reflect the updated data
            BindGridView();
        }
        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // Get the ID of the row being updated
            int rowID = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values["Room_ID"]);
            DeleteRow(rowID);
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            // Update the database with the new values

            String connectionString = ConfigurationManager.ConnectionStrings["HotelManagementSystemConnectionString"].ConnectionString;

            // Get the ID of the row being updated
            int rowID = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values["Room_ID"]);

            // Get the updated values for the RoomNumber, RoomType_ID, and Branch_ID fields
            string updatedRoomNo = ((TextBox)GridView1.Rows[e.RowIndex].FindControl("txtRoomNo")).Text;
            string updatedType = ((DropDownList)GridView1.Rows[e.RowIndex].FindControl("DropDownList5")).SelectedValue;
            string updatedBranch = ((DropDownList)GridView1.Rows[e.RowIndex].FindControl("DropDownList4")).SelectedValue;

            // Validate the updated RoomNumber
            string RoomNoError = "";
            int price;
            if (string.IsNullOrEmpty(updatedRoomNo))
            {
                RoomNoError = "RoomNO cannot be empty";
            }
            else if (!int.TryParse(updatedRoomNo, out price) || price <= 0)
            {
                RoomNoError = "Enter a positive RoomNO";
            }
            else
            {
                // Check if the updated RoomNumber already exists in the table for the same Branch_ID
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT COUNT(*) FROM Room WHERE RoomNumber = @RoomNumber AND Branch_ID = @Branch_ID AND Room_ID != @id";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@RoomNumber", updatedRoomNo);
                        command.Parameters.AddWithValue("@Branch_ID", updatedBranch);
                        command.Parameters.AddWithValue("@id", rowID);

                        int count = (int)command.ExecuteScalar();

                        if (count > 0)
                        {
                            RoomNoError = "Room Number already exists for the selected Branch";
                        }
                    }
                }
            }

            // Update the database if there are no errors
            if (string.IsNullOrEmpty(RoomNoError))
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "UPDATE Room SET RoomNumber = @RoomNumber, RoomType_ID = @Type, Branch_ID = @Branch_ID WHERE Room_ID = @id";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@RoomNumber", updatedRoomNo);
                        command.Parameters.AddWithValue("@Type", updatedType);
                        command.Parameters.AddWithValue("@Branch_ID", updatedBranch);
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
            else
            {
                // Display the RoomNoError message
                Label RoomNoErrorLabel = (Label)GridView1.Rows[e.RowIndex].FindControl("lblRoomNoError");
                if (RoomNoErrorLabel != null)
                {
                    RoomNoErrorLabel.Text = RoomNoError;
                    RoomNoErrorLabel.ForeColor = Color.Red;
                }
            }
        }


        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            searchText = SearchRoom.Text.Trim();
            Session["SearchText"] = searchText;
            GridView1.EditIndex = e.NewEditIndex;
            BindGridView();
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && GridView1.EditIndex == e.Row.RowIndex)
            {
                // Get the DropDownList control in the EditItemTemplate of the current row
                DropDownList ddlBranch = (DropDownList)e.Row.FindControl("DropDownList4");
                DropDownList ddlRoomType = (DropDownList)e.Row.FindControl("DropDownList5");

                // Bind the DropDownList with data from the database
                string connectionString = ConfigurationManager.ConnectionStrings["HotelManagementSystemConnectionString"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("SELECT Branch_ID FROM Hotel", conn);
                    conn.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    ddlBranch.DataSource = rdr;
                    ddlBranch.DataTextField = "Branch_ID";
                    ddlBranch.DataValueField = "Branch_ID";
                    ddlBranch.DataBind();
                    rdr.Close();

                    SqlCommand cmd1 = new SqlCommand("SELECT RoomType_ID FROM RoomType", conn);
                   
                    SqlDataReader rdr1 = cmd1.ExecuteReader();
                    ddlRoomType.DataSource = rdr1;
                    ddlRoomType.DataTextField = "RoomType_ID";
                    ddlRoomType.DataValueField = "RoomType_ID";
                    ddlRoomType.DataBind();
                    conn.Close();
                }

                // Set the selected value of the DropDownList to the current value of the Branch_ID field
                DataRowView drv = (DataRowView)e.Row.DataItem;
                ddlBranch.SelectedValue = drv["Branch_ID"].ToString();
                DataRowView drv1 = (DataRowView)e.Row.DataItem;
                ddlRoomType.SelectedValue = drv1["RoomType_ID"].ToString();
            }
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

        
        protected void idradio_CheckedChanged(object sender, EventArgs e)
        {
            SearchRoom.Attributes.Add("placeholder", "Room ID");
        }

        protected void roomradio_CheckedChanged(object sender, EventArgs e)
        {
            SearchRoom.Attributes.Add("placeholder", "Room Number");
        }

       

        protected void typeradio_CheckedChanged(object sender, EventArgs e)
        {
            SearchRoom.Attributes.Add("placeholder", "Room Type ID");
        }

      

        protected void branchradio_CheckedChanged(object sender, EventArgs e)
        {
            SearchRoom.Attributes.Add("placeholder", "Branch ID");
        }

    }
}