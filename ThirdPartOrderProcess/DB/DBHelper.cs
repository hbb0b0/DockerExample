
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;
using ThirdPartOrderProcess.Entity;
using ThirdPartOrderProcess.Model;
namespace ThirdPartOrderProcess.DB
{

    public class DBHelper
    {
        static  string s_ConnectionString;
        static List<OrderTemplateModel> s_OrderTemplateList = null;
        static DBHelper()
        {
           
        }

        internal static string ConnectionString
        {
           get
           {
               return s_ConnectionString;
           }
           set
           {
               s_ConnectionString =value;
           }
        }

        public static int Insert(OrderModel data)
        {
            using (SqlConnection sqlCon = new SqlConnection(s_ConnectionString))
            {
                int result = sqlCon.ExecuteScalar<int>(@"INSERT INTO [dbo].[Order]
           (
            [OrderTemplateID]
            ,[OrderNum]
           ,[JSON]
           ,[Status]
           ,[OrderStatus]
           ,[InDate])
     VALUES
           (
            @OrderTemplateID
            ,@OrderNum
           ,@JSON
           ,0
           ,@OrderStatus
           ,getdate())  SELECT CAST(SCOPE_IDENTITY() as int)", data);

                return result;
            }

        }

        public static List<OrderTemplateModel> GetOrderTemplateList()
        {
            if (s_OrderTemplateList == null)
            {
                #region sql
                string sql = @"
SELECT [ID]
      ,[TemplateName]
      ,[ResponseType]
      ,[SubmitSuccess]
      ,[SubmitFail]
      ,[SubmitFailRate]
      ,[QueryResponse]
      ,[NoticeParam]
      ,[NoticeURI]
      ,[NoticeURL]
      ,[NoticeSign]
      ,[SecretKey]
      ,[QueryBalance]
      ,[InDate]
  FROM [dbo].[Order_Template] ";
                #endregion
                using (SqlConnection sqlCon = new SqlConnection(s_ConnectionString))
                {
                    s_OrderTemplateList = sqlCon.Query<OrderTemplateModel>(sql).AsList();
                }
            }

            return s_OrderTemplateList;

        }

        public static List<OrderModel> GetWorkList()
        {

            #region sql
            string sql = @"
  Update [dbo].[Order]
  set [Status]=1
  output inserted.ID,inserted.InDate,inserted.JSON,inserted.OrderNum,inserted.OrderTemplateID,inserted.[Status]
  where ID in(
     Select top 20 ID 
	 from 
	 [dbo].[Order] with(nolock)
	 where 
	 OrderTemplateID=4
	 and 
	 [Status]=0
  )";
            #endregion
            List<OrderModel> result = new List<OrderModel>();
            using (SqlConnection sqlCon = new SqlConnection(s_ConnectionString))
            {
                result = sqlCon.Query<OrderModel>(sql).AsList();
            }
            return result;
        }

        public static OrderModel GetOrderByOrderNum(string orderNum)
        {
            string sql = @"SELECT [ID]
      ,[OrderTemplateID]
      ,[OrderNum]
      ,[JSON]
      ,[Status]
      ,[OrderStatus]
      ,[InDate]
  FROM [SUP_ThirdPartOrder].[dbo].[Order] with(nolock)
  WHERE OrderNum=@OrderNum";
            OrderModel result;
            using (SqlConnection sqlCon = new SqlConnection(s_ConnectionString))
            {
                result = sqlCon.QueryFirstOrDefault<OrderModel>(sql, new { OrderNum = orderNum });
            }

            return result;
        }

        public static OrderModel GetOrderByID(string transID)
        {
            string sql = @"SELECT [ID]
      ,[OrderTemplateID]
      ,[OrderNum]
      ,[JSON]
      ,[Status]
      ,[OrderStatus]
      ,[InDate]
  FROM [SUP_ThirdPartOrder].[dbo].[Order] with(nolock)
  WHERE ID=@ID";
            OrderModel result;
            using (SqlConnection sqlCon = new SqlConnection(s_ConnectionString))
            {
                result = sqlCon.QueryFirstOrDefault<OrderModel>(sql, new { ID = transID });
            }

            return result;
        }

        public static OrderModel UpdateOrderNoticeInfo(int id,string postResult)
        {
            string sql = @"Update [SUP_ThirdPartOrder].[dbo].[Order]
                  set NoticeResult= @NoticeResult,
                      NoticeTime=getdate()
                  WHERE ID=@ID  and NoticeResult is null
            ";
            OrderModel result;
            using (SqlConnection sqlCon = new SqlConnection(s_ConnectionString))
            {
                result = sqlCon.ExecuteScalar<OrderModel>(sql, new { ID = id, NoticeResult= postResult });
            }

            return result;
        }


    }

}