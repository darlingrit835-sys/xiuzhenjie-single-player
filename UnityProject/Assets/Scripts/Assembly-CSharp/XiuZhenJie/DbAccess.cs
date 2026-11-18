using System;
using Mono.Data.Sqlite;
using UnityEngine;

namespace XiuZhenJie
{
	public class DbAccess
	{
		public bool Connected;

		private SqliteConnection dbConnection;

		private SqliteCommand dbCommand;

		private SqliteDataReader reader;

		private string dbpath = Application.persistentDataPath + "/Config/XZJconfig.db";

		public DbAccess(string connectionString)
		{
			OpenDB(connectionString);
		}

		public DbAccess()
		{
			OpenDB(dbpath);
		}

		public void OpenDB(string connectionString)
		{
			try
			{
				dbConnection = new SqliteConnection("Data Source = " + connectionString);
				dbConnection.Open();
				Connected = true;
			}
			catch (Exception ex)
			{
				Connected = false;
				Debug.Log("连接本地数据库失败" + ex.Message);
			}
		}

		public void CloseSqlConnection()
		{
			try
			{
				if (dbCommand != null)
				{
					dbCommand.Dispose();
				}
				dbCommand = null;
				if (reader != null)
				{
					reader.Dispose();
				}
				reader = null;
				if (dbConnection != null)
				{
					dbConnection.Close();
				}
				dbConnection = null;
				Connected = false;
			}
			catch (Exception ex)
			{
				dbConnection = null;
				Connected = false;
				Debug.Log("关闭数据库失败." + ex.Message);
			}
		}

		public SqliteDataReader ExecuteQuery(string sqlQuery)
		{
			dbCommand = dbConnection.CreateCommand();
			dbCommand.CommandText = sqlQuery;
			reader = dbCommand.ExecuteReader();
			return reader;
		}

		public SqliteDataReader ReadFullTable(string tableName)
		{
			string sqlQuery = "SELECT * FROM " + tableName;
			return ExecuteQuery(sqlQuery);
		}

		public SqliteDataReader SelectCount(string tableName)
		{
			string sqlQuery = "SELECT COUNT(*) FROM " + tableName;
			return ExecuteQuery(sqlQuery);
		}

		public SqliteDataReader SelectWhere(string tableName, string[] items, string[] col, string[] operation, string[] values)
		{
			if (col.Length != operation.Length || operation.Length != values.Length)
			{
				throw new SqliteException("col.Length != operation.Length != values.Length");
			}
			string text = "SELECT " + items[0];
			for (int i = 1; i < items.Length; i++)
			{
				text = text + ", " + items[i];
			}
			text = text + " FROM " + tableName + " WHERE " + col[0] + operation[0] + "'" + values[0] + "' ";
			for (int j = 1; j < col.Length; j++)
			{
				text = text + " AND " + col[j] + operation[j] + "'" + values[0] + "' ";
			}
			return ExecuteQuery(text);
		}

		public SqliteDataReader SelectAnything(string tableName, string Selectsql, string Wheresql, string groupBysql = "", string Fromsql = "")
		{
			if (string.IsNullOrEmpty(tableName))
			{
				return null;
			}
			string text = "SELECT " + Selectsql;
			text = text + "FROM" + tableName + Fromsql + "WHERE" + Wheresql;
			text = text + "GROUP BY" + groupBysql;
			return ExecuteQuery(text);
		}

		public SqliteDataReader SelectAnythingBySql(string sql)
		{
			if (string.IsNullOrEmpty(sql))
			{
				return null;
			}
			return ExecuteQuery(sql);
		}

		public bool TableIsHave(string sql)
		{
			dbCommand = dbConnection.CreateCommand();
			dbCommand.CommandText = sql;
			if (Convert.ToInt32(dbCommand.ExecuteScalar()) == 0)
			{
				return false;
			}
			return true;
		}
	}
}
