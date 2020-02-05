using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Web.Security;

public static class FreeCycleUtils
{
    public static void AddOrendar(SqlConnection conn, int freecycle_id, int org_orendar_id)
    {
		SqlTransaction trans = conn.BeginTransaction();

		var sql2 = "select max(npp) + 1 from freecycle_orendar where freecycle_id = @freecycle_id";
		SqlCommand cmd2 = new SqlCommand(sql2, conn, trans);
		cmd2.Parameters.Add(new SqlParameter("freecycle_id", freecycle_id));
		var npp_o = cmd2.ExecuteScalar();
		var npp = (npp_o is DBNull ? 1 : (int)npp_o);

		var modified_by = Utils.GetUser();
		var modify_date = DateTime.Now;
		var sql = @"insert into [freecycle_orendar]([freecycle_id],[npp],[org_orendar_id],[modify_date],[modified_by])
					values(@freecycle_id, @npp, @org_orendar_id, @modify_date, @modified_by)";
		SqlCommand cmd = new SqlCommand(sql, conn, trans);
		cmd.Parameters.Add(new SqlParameter("freecycle_id", freecycle_id));
		cmd.Parameters.Add(new SqlParameter("npp", npp));
		cmd.Parameters.Add(new SqlParameter("org_orendar_id", org_orendar_id));
		cmd.Parameters.Add(new SqlParameter("modify_date", modify_date));
		cmd.Parameters.Add(new SqlParameter("modified_by", modified_by));
		cmd.ExecuteNonQuery();

		InsertEmptyRows(conn, trans, freecycle_id);

		trans.Commit();
	}


	public static void AddCycle(SqlConnection conn, SqlTransaction trans, int free_square_id)
	{
		var cycle_num = FreeCycleUtils.GetMaxCycleNum(conn, trans, free_square_id);
		cycle_num = (cycle_num == null ? 1 : cycle_num.Value + 1);

		var modified_by = Utils.GetUser();
		var modify_date = DateTime.Now;
		var start_date = DateTime.Now;
		var sql1 = @"insert into [freecycle]([free_square_id],[cycle_num],[start_date],[modify_date],[modified_by]) 
						values(@free_square_id, @cycle_num, @start_date, @modify_date, @modified_by)";
		SqlCommand cmd1 = new SqlCommand(sql1, conn, trans);
		cmd1.Parameters.Add(new SqlParameter("free_square_id", free_square_id));
		cmd1.Parameters.Add(new SqlParameter("cycle_num", cycle_num));
		cmd1.Parameters.Add(new SqlParameter("start_date", start_date));
		cmd1.Parameters.Add(new SqlParameter("modify_date", modify_date));
		cmd1.Parameters.Add(new SqlParameter("modified_by", modified_by));
		cmd1.ExecuteNonQuery();

		var freecycle_id = GetMaxFreeCycleId(conn, trans, free_square_id);
		InsertEmptyRows(conn, trans, freecycle_id.Value);
	}

	public static int? GetMaxCycleNum(SqlConnection conn, SqlTransaction trans, int free_square_id)
	{
		var sql3 = "select top 1 [cycle_num] from [freecycle] where [free_square_id] = @free_square_id and [is_deleted] = 0 order by [cycle_num] desc";
		SqlCommand cmd3 = new SqlCommand(sql3, conn, trans);
		cmd3.Parameters.Add(new SqlParameter("free_square_id", free_square_id));
		var cycle_num_o = cmd3.ExecuteScalar();
		var cycle_num = (cycle_num_o == null || cycle_num_o is DBNull ? (int?)null : (int)cycle_num_o);
		return cycle_num;
	}

	public static int? GetMaxFreeCycleId(SqlConnection conn, SqlTransaction trans, int free_square_id)
	{
		var sql2 = "select top 1 freecycle_id from [freecycle] where free_square_id = @free_square_id and [is_deleted] = 0 order by cycle_num desc";
		SqlCommand cmd2 = new SqlCommand(sql2, conn, trans);
		cmd2.Parameters.Add(new SqlParameter("free_square_id", free_square_id));
		var freecycle_id_o = cmd2.ExecuteScalar();
		var freecycle_id = (freecycle_id_o == null || freecycle_id_o is DBNull ? (int?)null : (int)freecycle_id_o);
		return freecycle_id;
	}



	public static void InsertEmptyRows(SqlConnection conn, SqlTransaction trans, int freecycle_id)
	{
		var sql = @"insert into [freecycle_step]([freecycle_id],[freecycle_orendar_id],[step_id]) 
					select A.freecycle_id, B.freecycle_orendar_id, C.step_id from [freecycle] A, [freecycle_orendar] B, [freecycle_step_dict] C 
					where A.freecycle_id = B.freecycle_id and A.freecycle_id = @freecycle_id
					and not exists (select 1 from [freecycle_step] Q where Q.freecycle_id = A.freecycle_id and Q.freecycle_orendar_id = B.freecycle_orendar_id and Q.step_id = C.step_id)";
		SqlCommand cmd = new SqlCommand(sql, conn, trans);
		cmd.Parameters.Add(new SqlParameter("freecycle_id", freecycle_id));
		cmd.ExecuteNonQuery();
	}

	public static void UpdateStageInfo(SqlConnection conn, SqlTransaction trans, int free_square_id)
	{
		var sql1 = @"
update reports1nf_balans_free_square 
set freecycle_step_dict_id = T.step_id, current_stage_docdate = T.step_date, current_stage_docnum = T.step_docnum
from reports1nf_balans_free_square R
outer apply
(
	select 
	top 1
	A.step_date,
	A.step_docnum,
	A.step_id
	from freecycle_step A 
	left join freecycle_step_dict B on B.step_id = A.step_id
	where 1=1
	and A.freecycle_id = (select top 1 Q.freecycle_id from [freecycle] Q where free_square_id = R.id and Q.is_deleted = 0 order by Q.cycle_num desc)
	and A.step_date is not null
	and A.is_deleted = 0
	order by step_date desc, B.step_ord desc
) T
where id = @free_square_id";
		SqlCommand cmd1 = new SqlCommand(sql1, conn, trans);
		cmd1.Parameters.Add(new SqlParameter("free_square_id", free_square_id));
		cmd1.ExecuteNonQuery();
	}
}