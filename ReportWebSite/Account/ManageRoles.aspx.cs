using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.Data;

public class RoleDataSource
{
    public string roleName = "";

    public RoleDataSource(string name)
    {
        roleName = name;
    }

    public string RoleName
    {
        get
        {
            return roleName;
        }

        set
        {
            roleName = value;
        }
    }
}

public partial class Account_ManageRoles : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        GridViewRoles.DataSource = CreateRolesDataSource(Roles.GetAllRoles());
        GridViewRoles.DataBind();

        ComboRolesToAdd.DataSource = Roles.GetAllRoles();
        ComboRolesToAdd.DataBind();

        ComboUsers.DataSource = Membership.GetAllUsers();
        ComboUsers.DataBind();

        // Bind the grid of user roles
        int selectedUser = ComboUsers.SelectedIndex;

        if (selectedUser >= 0)
        {
            string userName = ComboUsers.Items[selectedUser].ToString();

            GridViewUserRoles.DataSource = CreateRolesDataSource(Roles.GetRolesForUser(userName));
        }
        else
        {
            GridViewUserRoles.DataSource = null;
        }

        GridViewUserRoles.DataBind();
    }

    protected void ButtonAddRole_Click(object sender, EventArgs e)
    {
        string newRoleName = EditNewRoleName.Text.Trim();

        if (!Roles.RoleExists(newRoleName))
        {
            // Create the role
            Roles.CreateRole(newRoleName);
        }

        EditNewRoleName.Text = string.Empty;

        // Rebind the grid of roles
        GridViewRoles.DataSource = CreateRolesDataSource(Roles.GetAllRoles());
        GridViewRoles.DataBind();
    }

    protected void GridViewRoles_RowDeleting(object sender, ASPxDataDeletingEventArgs e)
    {
        // Get the RoleName
        object rowKey = e.Keys[GridViewRoles.KeyFieldName];

        if (rowKey is string)
        {
            string roleName = (string)rowKey;

            // Delete the role
            Roles.DeleteRole(roleName, false);

            // Rebind the grid of roles
            GridViewRoles.DataSource = CreateRolesDataSource(Roles.GetAllRoles());
            GridViewRoles.DataBind();
        }

        e.Cancel = true;
    }

    protected void GridViewUserRoles_RowDeleting(object sender, ASPxDataDeletingEventArgs e)
    {
        int selectedUser = ComboUsers.SelectedIndex;

        if (selectedUser >= 0)
        {
            // Get the RoleName
            object rowKey = e.Keys[GridViewUserRoles.KeyFieldName];

            if (rowKey is string)
            {
                string userName = ComboUsers.Items[selectedUser].ToString();
                string roleName = (string)rowKey;

                if (Roles.IsUserInRole(userName, roleName))
                {
                    Roles.RemoveUserFromRole(userName, roleName);

                    GridViewUserRoles.DataSource = CreateRolesDataSource(Roles.GetRolesForUser(userName));
                    GridViewUserRoles.DataBind();
                }
            }
        }

        e.Cancel = true;
    }

    protected void ComboUsers_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void ButtonAssignRole_Click(object sender, EventArgs e)
    {
        int selectedUser = ComboUsers.SelectedIndex;
        int roleIndex = ComboRolesToAdd.SelectedIndex;

        if (selectedUser >= 0 && roleIndex >= 0)
        {
            string userName = ComboUsers.Items[selectedUser].ToString();
            string roleName = ComboRolesToAdd.Items[roleIndex].ToString();

            if (!Roles.IsUserInRole(userName, roleName))
            {
                Roles.AddUserToRole(userName, roleName);

                GridViewUserRoles.DataSource = CreateRolesDataSource(Roles.GetRolesForUser(userName));
                GridViewUserRoles.DataBind();
            }
        }
    }

    protected System.Collections.ArrayList CreateRolesDataSource(string[] roleNames)
    {
        System.Collections.ArrayList dataSource = new System.Collections.ArrayList();

        for (int i = 0; i < roleNames.Length; i++)
        {
            dataSource.Add(new RoleDataSource(roleNames[i]));
        }

        return dataSource;
    }
}