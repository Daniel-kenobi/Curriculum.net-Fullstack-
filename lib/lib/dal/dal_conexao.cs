using System;
using System.Data;
using System.Data.Common;
using MySql.Data.MySqlClient;
using FirebirdSql.Data.FirebirdClient;
using System.IO;
using lib.sistema;

namespace lib.dal
{

    public enum lib_SGDB
    {
        Firebird = 0,
        MySQL = 1
    }

    public class dt_conexao
    {
        public string hst { get; set; }
        public string bd { get; set; }
        public string usr { get; set; }
        public string shn { get; set; }
        public lib_SGDB SGDB { get; set; }
    }

    /*==========================================================================*/
    /*===========================     Firebird     =============================*/
    /*==========================================================================*/

    public class dal_conexao_fb
    {
        private dt_conexao obj_conexao;

        private int intTantativas = 3; // tentativas de acesso ao banco de dados

        public string ConnectionStringServidor;

        public FbCommand cmd;
        public FbConnection conn;
        public FbDataReader Reader;
        public FbTransaction Trans;
        public dal_conexao_fb(dt_conexao adtc)
        {
            obj_conexao = adtc;
        }

        public void OpenConn()
        {
            string err_msg = "";
            conn = new FbConnection();

            ConnectionStringServidor = "User=" + obj_conexao.usr + "; Password=" + obj_conexao.shn + "; Role=3; " +
                "Datasource=" + obj_conexao.hst + "; Database=" + obj_conexao.bd + "; Server type=0;" +
                "Port=3050;Dialect=3;Charset=NONE;Connection lifetime=15;Connection timeout=20;Pooling=true;MinPoolSize=1;";

            conn.ConnectionString = ConnectionStringServidor;

            for (int i = 0; i < intTantativas; i++)
            {
                try
                {
                    if (this.conn.State == ConnectionState.Closed)
                        conn.Open();
                    break;
                }
                catch (Exception err)
                {
                    System.Threading.Thread.Sleep(5000);
                    err_msg = err.Message;
                }
            }

            if (this.conn.State == ConnectionState.Open)
                this.Trans = this.conn.BeginTransaction(IsolationLevel.ReadCommitted);
            else
                throw new Exception(err_msg);

        }

        public void CloseConn(bool PConfirmar = true)
        {
            try
            {

                if (this.conn != null)

                    if (this.conn.State != ConnectionState.Closed)
                    {
                        if (PConfirmar)
                        {
                            this.Trans.Commit();
                        }
                        else
                        {
                            this.Trans.Rollback();
                        }
                    }

            }
            catch
            {
                throw;
            }
            finally
            {
                this.Trans.Dispose();
                if (this.conn.State != ConnectionState.Closed)
                    conn.Close();
                conn.Dispose();
            }
        }

        public void AddParams(string Parametro, object Valor)
        {
            this.cmd.Parameters.AddWithValue(Parametro, Valor);
        }

        public bool CreateCommand(string pSQL)
        {
            try
            {
                this.cmd = new FbCommand(pSQL, this.conn);
                this.cmd.Transaction = this.Trans;
                this.cmd.Prepare();
                this.cmd.CommandTimeout = 6000;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void ExecSQL()
        {
            this.cmd.ExecuteNonQuery();
        }

        public bool ExecReader()
        {
            this.Reader = this.cmd.ExecuteReader();
            return true;
        }

        public void CloseReader()
        {
            if (this.Reader != null)
            {
                if (!this.Reader.IsClosed)
                    this.Reader.Close();
            }
        }

        public void ConectarBD()
        {
            string err_msg = "";
            conn = new FbConnection();

            ConnectionStringServidor = "server=" + obj_conexao.hst + "; database=" + obj_conexao.bd +
                ";uid= " + obj_conexao.usr + "; pwd= " + obj_conexao.shn + "; " +
                "connectionTimeout=18000; DefaultCommandTimeout=18000;" +
                "Pooling=true; MinPoolSize=1; ";

            conn.ConnectionString = ConnectionStringServidor;

            for (int i = 0; i < intTantativas; i++)
            {
                try
                {
                    if (this.conn.State == ConnectionState.Closed)
                        conn.Open();

                    break;
                }
                catch (Exception err)
                {
                    System.Threading.Thread.Sleep(5000);
                    err_msg = err.Message;
                }
            }

            if (this.conn.State != ConnectionState.Open)
                throw new Exception(err_msg);

        }

        public void DesconectarBD()
        {
            try
            {

                if (this.conn.State != ConnectionState.Closed)
                    conn.Close();

            }
            catch
            {
                throw;
            }
            finally
            {

                conn.Dispose();
            }
        }

        public void IniciarTransacao()
        {

            if (this.conn.State == ConnectionState.Open)
                this.Trans = this.conn.BeginTransaction(IsolationLevel.ReadCommitted);
            else
                throw new Exception("Falha ao iniciar transação.");

        }

        public void FinalizarTransacao(bool PConfirmar = true)
        {

            try
            {

                if (this.conn != null)

                    if (this.conn.State != ConnectionState.Closed)
                    {
                        if (PConfirmar)
                        {
                            this.Trans.Commit();
                        }
                        else
                        {
                            this.Trans.Rollback();
                        }
                    }

            }
            catch
            {
                throw;
            }
            finally
            {
                this.Trans.Dispose();
            }

        }

    }


    /*==========================================================================*/
    /*=============================      MySQL     =============================*/
    /*==========================================================================*/

    public class dal_conexao_MySQL
    {

        private dt_conexao obj_conexao;

        private int intTantativas = 3; // tentativas de acesso ao banco de dados

        public MySqlCommand cmd;
        public MySqlConnection conn;
        public string ConnectionStringServidor;
        public MySqlDataReader Reader;
        public MySqlTransaction Trans;

        public dal_conexao_MySQL(dt_conexao adtc)
        {
            obj_conexao = adtc;
        }

        public void Dispose()
        {

            this.cmd = null;
            this.conn = null;
            this.Reader = null;
            this.Trans = null;

            obj_conexao = null;

        }

        public void OpenConn()
        {
            string err_msg = "";
            conn = new MySqlConnection();

            ConnectionStringServidor = "server=" + obj_conexao.hst + "; database=" + obj_conexao.bd +
                ";uid= " + obj_conexao.usr + "; pwd= " + obj_conexao.shn + "; " +
                "connectionTimeout=28000; DefaultCommandTimeout=28000;" +
                "Pooling=true; MinPoolSize=1; UseCompression=True; SSL Mode=None;";
            //UseCompression=True;

            conn.ConnectionString = ConnectionStringServidor;

            for (int i = 0; i < intTantativas; i++)
            {
                try
                {
                    if (this.conn.State == ConnectionState.Closed)
                        conn.Open();

                    break;
                }
                catch (Exception err)
                {
                    System.Threading.Thread.Sleep(5000);
                    err_msg = err.Message;
                }
            }

            if (this.conn.State == ConnectionState.Open)
                this.Trans = this.conn.BeginTransaction(IsolationLevel.ReadCommitted);
            else
                throw new Exception(err_msg);

        }

        public void CloseConn(bool PConfirmar = true)
        {
            try
            {

                if (this.conn != null)

                    if (this.conn.State != ConnectionState.Closed)
                    {
                        if (PConfirmar)
                        {
                            this.Trans.Commit();
                        }
                        else
                        {
                            this.Trans.Rollback();
                        }
                    }

            }
            catch
            {
                throw;
            }
            finally
            {
                this.Trans.Dispose();
                if (this.conn.State != ConnectionState.Closed)
                    conn.Close();
                conn.Dispose();
            }
        }

        public void AddParams(string Parametro, object Valor)
        {
            this.cmd.Parameters.AddWithValue(Parametro, Valor);
        }

        public bool CreateCommand(string pSQL)
        {
            try
            {
                this.cmd = new MySqlCommand(pSQL, this.conn);
                this.cmd.Prepare();
                this.cmd.CommandTimeout = 6000;
                this.cmd.Transaction = this.Trans;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void ExecSQL()
        {
            this.cmd.ExecuteNonQuery();
        }

        public bool ExecReader()
        {
            this.Reader = this.cmd.ExecuteReader();
            return true;
        }

        public void CloseReader()
        {
            if (this.Reader != null)
            {
                if (!this.Reader.IsClosed)
                    this.Reader.Close();
            }
        }

        public void ConectarBD()
        {
            string err_msg = "";
            conn = new MySqlConnection();

            ConnectionStringServidor = "server=" + obj_conexao.hst + "; database=" + obj_conexao.bd +
                ";uid= " + obj_conexao.usr + "; pwd= " + obj_conexao.shn + "; " +
                "connectionTimeout=28000; DefaultCommandTimeout=28000;" +
                "Pooling=true; MinPoolSize=1; UseCompression=True; SSL Mode=None;";

            conn.ConnectionString = ConnectionStringServidor;

            for (int i = 0; i < intTantativas; i++)
            {
                try
                {
                    if (this.conn.State == ConnectionState.Closed)
                        conn.Open();

                    break;
                }
                catch (Exception err)
                {
                    System.Threading.Thread.Sleep(5000);
                    err_msg = err.Message;
                }
            }

            if (this.conn.State != ConnectionState.Open)
                throw new Exception(err_msg);

        }

        public void DesconectarBD()
        {
            try
            {

                if ((this?.conn?.State ?? ConnectionState.Closed) != ConnectionState.Closed)
                    conn?.Close();

            }
            catch
            {
                throw;
            }
            finally
            {

                conn?.Dispose();
            }
        }

        public void IniciarTransacao()
        {

            if (this.conn.State == ConnectionState.Open)
                this.Trans = this.conn.BeginTransaction(IsolationLevel.ReadCommitted);
            else
                throw new Exception("Falha ao iniciar transação.");

        }

        public void FinalizarTransacao(bool PConfirmar = true)
        {

            try
            {

                if (this.conn != null)

                    if (this.conn.State != ConnectionState.Closed)
                    {
                        if (PConfirmar)
                        {
                            this.Trans.Commit();
                        }
                        else
                        {
                            this.Trans.Rollback();
                        }
                    }

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                this?.Trans?.Dispose();
            }

        }

    }

    /*==========================================================================*/
    /*===========================     Interface     ============================*/
    /*==========================================================================*/

    public class dal_conexao
    {

        private dt_conexao obj_conexao;

        private dal_conexao_fb cnx_fb;
        private dal_conexao_MySQL cnx_My;

        public DbDataReader Reader
        {
            get
            {

                DbDataReader rst = null;

                switch (obj_conexao.SGDB)
                {
                    case lib_SGDB.MySQL:
                        rst = cnx_My.Reader;
                        break;

                    case lib_SGDB.Firebird:
                        rst = cnx_fb.Reader;
                        break;
                }

                return rst;
            }
        }

        public dal_conexao(dt_conexao adtc = null)
        {

            if (adtc == null)
            {
                if (!File.Exists(Environment.CurrentDirectory + "//conexao.json"))
                    throw new Exception("Arquivo de configuração de conexão inexistente");

                adtc = fncComuns.fnc_converter_json<dt_conexao>(File.ReadAllText(Environment.CurrentDirectory + "//conexao.json"));
            }

            obj_conexao = adtc;

            switch (obj_conexao.SGDB)
            {

                case lib_SGDB.MySQL:
                    cnx_My = new dal_conexao_MySQL(obj_conexao);
                    break;

                case lib_SGDB.Firebird:
                    cnx_fb = new dal_conexao_fb(obj_conexao);
                    break;
            }

        }

        public void OpenConn(bool novaConexao = true)
        {
            if (!novaConexao)
                return;

            switch (obj_conexao.SGDB)
            {

                case lib_SGDB.MySQL:
                    cnx_My.OpenConn();
                    break;

                case lib_SGDB.Firebird:
                    cnx_fb.OpenConn();
                    break;
            }

        }

        public void CloseConn(bool PConfirmar = true, bool fecharConexao = true)
        {
            if (!fecharConexao)
                return;

            switch (obj_conexao.SGDB)
            {

                case lib_SGDB.MySQL:
                    cnx_My.CloseConn(PConfirmar);
                    break;

                case lib_SGDB.Firebird:
                    cnx_fb.CloseConn(PConfirmar);
                    break;
            }
        }

        public void AddParams(string Parametro, object Valor)
        {
            switch (obj_conexao.SGDB)
            {

                case lib_SGDB.MySQL:
                    cnx_My.AddParams(Parametro, Valor);
                    break;

                case lib_SGDB.Firebird:
                    cnx_fb.AddParams(Parametro, Valor);
                    break;
            }
        }

        public bool CreateCommand(string pSQL)
        {
            bool rst = false;
            switch (obj_conexao.SGDB)
            {

                case lib_SGDB.MySQL:
                    rst = cnx_My.CreateCommand(pSQL);
                    break;

                case lib_SGDB.Firebird:
                    rst = cnx_fb.CreateCommand(pSQL);
                    break;
            }
            return rst;
        }

        public void ExecSQL()
        {
            switch (obj_conexao.SGDB)
            {

                case lib_SGDB.MySQL:
                    cnx_My.ExecSQL();
                    break;

                case lib_SGDB.Firebird:
                    cnx_fb.ExecSQL();
                    break;
            }
        }

        public bool ExecReader()
        {
            bool rst = false;
            switch (obj_conexao.SGDB)
            {

                case lib_SGDB.MySQL:
                    rst = cnx_My.ExecReader();
                    break;

                case lib_SGDB.Firebird:
                    rst = cnx_fb.ExecReader();
                    break;
            }
            return rst;
        }

        public void CloseReader()
        {
            switch (obj_conexao.SGDB)
            {

                case lib_SGDB.MySQL:
                    cnx_My.CloseReader();
                    break;

                case lib_SGDB.Firebird:
                    cnx_fb.CloseReader();
                    break;
            }
        }

        public void ConectarBD(bool novaConexao = true)
        {
            if (!novaConexao)
                return;

            switch (obj_conexao.SGDB)
            {

                case lib_SGDB.MySQL:
                    cnx_My.ConectarBD();
                    break;

                case lib_SGDB.Firebird:
                    cnx_fb.ConectarBD();
                    break;
            }
        }

        public void DesconectarBD(bool fecharConexao = true)
        {
            if (!fecharConexao)
                return;

            switch (obj_conexao.SGDB)
            {

                case lib_SGDB.MySQL:
                    cnx_My.DesconectarBD();
                    break;

                case lib_SGDB.Firebird:
                    cnx_fb.DesconectarBD();
                    break;
            }
        }

        public void IniciarTransacao()
        {

            switch (obj_conexao.SGDB)
            {

                case lib_SGDB.MySQL:
                    cnx_My.IniciarTransacao();
                    break;

                case lib_SGDB.Firebird:
                    cnx_fb.IniciarTransacao();
                    break;
            }

        }

        public void FinalizarTransacao(bool PConfirmar = true)
        {

            switch (obj_conexao.SGDB)
            {

                case lib_SGDB.MySQL:
                    cnx_My.FinalizarTransacao(PConfirmar);
                    break;

                case lib_SGDB.Firebird:
                    cnx_fb.FinalizarTransacao(PConfirmar);
                    break;
            }

        }

    }

}