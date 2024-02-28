using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Data.SqlClient;

namespace Common
{
    public class Common
    {
        #region "****"
        /// <summary>
        /// テスト　
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        static public DataTable test(SqlCommand cmd)
        {

            StringBuilder sql = new StringBuilder();
            sql.AppendLine(" SELECT  ");

            cmd.CommandText = sql.ToString();

            //  SELECTしてDataTableに取り込む
            var dataAdapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            dataAdapter.Fill(dt);

            return (dt);
        }
        //SQLで取得したレコードをTに入れてる　採用しない場合はこのメソッド削除
        public List<T> PutValuesIntoModel<T>(SqlCommand command)
        {
            List<T> dtoList = new List<T>();

            // SQLを実行
            // usingブロックが終わり次第、コネクションを閉じるようにCommandBehavior.CloseConnectionを指定
            using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
            {
                // 行データ分のループ
                while (reader.Read())
                {
                    // Tで指定された総称型のインスタンスを生成（DTOの作成）
                    T dtoObject = (T)Activator.CreateInstance(typeof(T));

                    foreach (PropertyInfo property in dtoObject.GetType().GetProperties())
                    {
                        try
                        {
                            // 取得したSQLの項目が指定したモデルに存在する場合のみ処理を実装する
                            if (Enumerable.Range(0, reader.FieldCount).Select(i => reader.GetName(i)).ToArray().Contains(property.Name))
                            {
                                if (!DBNull.Value.Equals(reader[property.Name]))
                                {
                                    // プロパティ（フィールド）名を元に、SqlDataReaderから値を取得し、DTOにセット
                                    property.SetValue(dtoObject, reader[property.Name], null);
                                }
                            }
                        }
                        catch (IndexOutOfRangeException ex)
                        {
                            Debug.Print(property.Name + " was warn");
                            Debug.Print(ex.Message);
                            continue;
                        }
                        catch (Exception ex)
                        {
                            Debug.Print(property.Name + " was error");
                            Debug.Print(ex.Message);
                            throw;
                        }

                    }
                    dtoList.Add(dtoObject);
                }
            }
            return dtoList;
        }



        #endregion
    }

}
