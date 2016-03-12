/*
 */
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Data;
using System.IO;

namespace funfiscal
{
	public class tfhka
	{
		public tfhka()
		{
		}

		// Bixolon, Aclas, OKI, BMC
		[DllImport("TFHKAIF.DLL")] private static extern int OpenFpctrl(string portname);
		[DllImport("TFHKAIF.DLL")] private static extern int CloseFpctrl();
		[DllImport("TFHKAIF.DLL")] private static extern int CheckFprinter();
		[DllImport("TFHKAIF.DLL")] private static extern int ReadFpStatus(ref long lstatus,ref long lerror);
		[DllImport("TFHKAIF.DLL")] private static extern int SendCmd(ref long lstatus,ref long lerror,string cmd);
		[DllImport("TFHKAIF.DLL")] private static extern int SendNCmd(ref long lstatus,ref long lerror,ref string buffercmd);
		[DllImport("TFHKAIF.DLL")] private static extern int SendFileCmd(ref long lstatus,ref long lerror,ref string filecmd);
		[DllImport("TFHKAIF.DLL")] private static extern int UploadReportCmd(ref long lstatus,ref long lerror,string cmd,string filecmd);
		[DllImport("TFHKAIF.DLL")] private static extern int UploadStatusCmd(ref long lstatus,ref long lerror,string cmd,string filecmd);

		public static bool GeneraFactura(string codfiscal,string puerto,string referencia,
		                                 string rifoci,string nomcli,string dircli,
		                                 string tlfcli,string vendedor,string condipago,
		                                 decimal totaldoc,DateTime fechavence,
		                                 DataTable dtDetalle,DataTable dtPagos,
		                                 string checkin,string huesped,string habino,
		                                 string placa,string cajero,string estacion)
		{
			int    respuesta;
			long   status = 0;
			long   error  = 0;
			string cadena;
			bool   ok     = false;

			string descrip, cant, precio, talla, color, seriales, notas;
			char[] tipotasa = new Char[4];
			int    i;

			tipotasa[0] = ' ';
			tipotasa[1] = '!';
			tipotasa[2] = '"';
			tipotasa[3] = '#';

			if (false)
			{
				abrir_gaveta(puerto);
			}

			if (abrir_puerto(puerto) )
		    {
				rifoci    = rifoci.Substring(0,(rifoci.Length<12?rifoci.Length:12)).Trim();
				nomcli    = nomcli.Substring(0,(nomcli.Length<80?nomcli.Length:80)).Trim();
				dircli    = dircli.Trim();

				if (codfiscal == "BIXOLON350" || codfiscal == "DASCOM230")
				{
					cadena    = "jS"+nomcli;
	     			respuesta = SendCmd(ref status,ref error,cadena);

					cadena    = "jR"+rifoci;
	     			respuesta = SendCmd(ref status,ref error,cadena);

	     			cadena    = "j1"+"DIRECCION:"+dircli;
	     			respuesta = SendCmd(ref status,ref error,cadena);

	     			cadena    = "j2"+"REF. "+referencia+" / "+cajero+" / "+estacion;
	     			respuesta = SendCmd(ref status,ref error,cadena);

	     			cadena    = "j3"+"VEND."+vendedor;
	     			respuesta = SendCmd(ref status,ref error,cadena);

					if (!string.IsNullOrEmpty(huesped))
					{
		     			cadena    = "j4"+"HUESPED: "+huesped;
		     			respuesta = SendCmd(ref status,ref error,cadena);
					}

					if (!string.IsNullOrEmpty(habino))
					{
		     			cadena    = "j5"+"HAB. No: "+habino;
		     			respuesta = SendCmd(ref status,ref error,cadena);
					}
				}
				else
				{
					cadena    = "i01 CLIENTE:"+nomcli;
	     			respuesta = SendCmd(ref status,ref error,cadena);
	
					cadena    = "i02 CI/RIF :"+rifoci;
	     			respuesta = SendCmd(ref status,ref error,cadena);
	
	     			cadena    = "i03 DIRECCION:"+dircli;
	     			respuesta = SendCmd(ref status,ref error,cadena);
	
	     			cadena    = "i04 TELEFONO :"+tlfcli;
	     			respuesta = SendCmd(ref status,ref error,cadena);
	
	     			cadena    = "i05 REF. "+referencia+" / "+cajero+" / "+estacion;
	     			respuesta = SendCmd(ref status,ref error,cadena);
	
	     			cadena    = "i06 VEND."+vendedor;
	     			respuesta = SendCmd(ref status,ref error,cadena);
	
					if (!string.IsNullOrEmpty(huesped))
					{
		     			cadena    = "i07 HUESPED: "+huesped;
		     			respuesta = SendCmd(ref status,ref error,cadena);
					}
	
					if (!string.IsNullOrEmpty(habino))
					{
		     			cadena    = "i08 HAB. No: "+habino;
		     			respuesta = SendCmd(ref status,ref error,cadena);
					}
				}

				foreach (DataRow row in dtDetalle.Rows)
				{
					descrip   = row["codigo"].ToString().Trim()+" "+row["descrip"].ToString();
					cant      = Convert.ToDecimal(row["cant"]).ToString("###0.000");
					cant      = cant.Replace(",","").Replace(".","");
					cant      = cant.PadLeft(8,'0');
					precio    = Convert.ToDecimal(row["precio"]).ToString("#####0.00");
					precio    = precio.Replace(",","").Replace(".","");
					precio    = precio.PadLeft(10,'0');
					talla     = row["talla"].ToString();
					color     = row["color"].ToString();
					seriales  = row["seriales"].ToString();
					notas     = row["notas"].ToString();
					i         = Convert.ToInt16(row["baseiva"]);

					cadena    = tipotasa[i]+precio+cant+descrip;
					respuesta = SendCmd(ref status,ref error,cadena);

					if (!string.IsNullOrEmpty(talla))
					{
		     			cadena    = "@ "+talla+" / "+color;
		     			respuesta = SendCmd(ref status,ref error,cadena);
					}

					if (!string.IsNullOrEmpty(seriales))
					{
		     			cadena    = "@ S/N:"+seriales;
		     			respuesta = SendCmd(ref status,ref error,cadena);
					}
				}

				bool    pagodirecto = true;
				decimal monto, topago = 0, tofact; 
				string  montopago;

				cadena = "101";

				if (dtPagos != null)
				{
					if (dtPagos.Rows.Count == 1)
					{
						if (dtPagos.Rows[0]["idpagofiscal"].ToString().Trim() != "")
						{
							cadena = "1"+dtPagos.Rows[0]["idpagofiscal"].ToString().Trim();
						}
					}
					else
					{
						foreach (DataRow row in dtPagos.Rows)
						{
							monto = Convert.ToDecimal(row["monto"]);

							if (monto > 0 && row["idpagofiscal"].ToString().Trim() != "")
							{
								montopago   = monto.ToString("#####0.00");
								montopago   = montopago.Replace(",","").Replace(".","");
								montopago   = montopago.PadLeft(12,'0');
		
								cadena      = "2"+row["idpagofiscal"].ToString().Trim()+montopago;
				     			respuesta   = SendCmd(ref status,ref error,cadena);
		
								pagodirecto = false;
								topago      += monto;
							}
						}

						if (topago > 0)
						{
							cadena = LeerStatusS2(true,51,13);

							tofact = (cadena != "" ? (Convert.ToDecimal(cadena) / 100) : totaldoc);

							if (topago < tofact)
							{
								monto     = (tofact - topago);
								montopago = monto.ToString("#####0.00");
								montopago = montopago.Replace(",","").Replace(".","");
								montopago = montopago.PadLeft(12,'0');
			
								cadena    = "201"+montopago;
					     		respuesta = SendCmd(ref status,ref error,cadena);
							}
						}
					}
				}

				if (pagodirecto)
				{
					respuesta = SendCmd(ref status,ref error,cadena);
				}

				ok  = evalua_errores(respuesta);

				CloseFpctrl();
		    }

			return(ok);
		}
	
		public static bool GeneraNotaCredito(string codfiscal,string puerto,
		                                     string referencia,string rifoci,
		                                     string nomcli,string dircli,string tlfcli,
		                                     string vendedor,DataTable dtDetalle,
		                                     string checkin,string huesped,string habino,
		                                     string placa,string cajero,string estacion,
		                                     string facturaNC,DateTime fechaFactura,
		                                     string serialFiscal)
		{
			int    respuesta;
			long   status = 0;
			long   error  = 0;
			string cadena;
			bool   ok     = false;

			string fecha, descrip, cant, precio, talla, color, seriales, notas;
			char[] tipotasa = new Char[4];
			int    i;

			tipotasa[0] = '0';
			tipotasa[1] = '1';
			tipotasa[2] = '2';
			tipotasa[3] = '3';

			if (false)
			{
				abrir_gaveta(puerto);
			}

			if (abrir_puerto(puerto) )
		    {
				rifoci    = rifoci.Substring(0,(rifoci.Length<12?rifoci.Length:12)).Trim();
				nomcli    = nomcli.Substring(0,(nomcli.Length<80?nomcli.Length:80)).Trim();
				dircli    = dircli.Trim();
				fecha     = fechaFactura.ToString("dd-MM-yyyy");

				if (codfiscal == "BIXOLON350" || codfiscal == "DASCOM230")
				{
					cadena    = "jS"+nomcli;
	     			respuesta = SendCmd(ref status,ref error,cadena);

					cadena    = "jR"+rifoci;
	     			respuesta = SendCmd(ref status,ref error,cadena);

					cadena    = "jF*"+facturaNC;
	     			respuesta = SendCmd(ref status,ref error,cadena);

	     			cadena    = "j1"+"Fecha :"+fecha+"  Serial :"+serialFiscal;
	     			respuesta = SendCmd(ref status,ref error,cadena);

	     			cadena    = "j2"+"REF. "+referencia+" / "+cajero+" / "+estacion;
	     			respuesta = SendCmd(ref status,ref error,cadena);

					if (!string.IsNullOrEmpty(huesped))
					{
		     			cadena    = "j3"+"HUESPED: "+huesped;
		     			respuesta = SendCmd(ref status,ref error,cadena);
					}

					if (!string.IsNullOrEmpty(habino))
					{
		     			cadena    = "j4"+"HAB. No: "+habino;
		     			respuesta = SendCmd(ref status,ref error,cadena);
					}
				}
				else
				{
					cadena    = "i01 CLIENTE:"+nomcli;
	     			respuesta = SendCmd(ref status,ref error,cadena);

					cadena    = "i02 CI/RIF :"+rifoci;
	     			respuesta = SendCmd(ref status,ref error,cadena);

	     			cadena    = "i03 DIRECCION:"+dircli;
	     			respuesta = SendCmd(ref status,ref error,cadena);

	     			cadena    = "i04 TELEFONO :"+tlfcli;
	     			respuesta = SendCmd(ref status,ref error,cadena);

	     			cadena    = "i05 Factura :"+facturaNC+"  Fecha :"+fecha;
	     			respuesta = SendCmd(ref status,ref error,cadena);

		     		cadena    = "i06 REF. "+referencia+" / "+cajero+" / "+estacion;
		     		respuesta = SendCmd(ref status,ref error,cadena);

					if (!string.IsNullOrEmpty(huesped))
					{
						cadena    = "i07 HUESPED: "+huesped;
		     			respuesta = SendCmd(ref status,ref error,cadena);
					}
	
					if (!string.IsNullOrEmpty(habino))
					{
		     			cadena    = "i08 HAB. No: "+habino;
		     			respuesta = SendCmd(ref status,ref error,cadena);
					}
				}

				foreach (DataRow row in dtDetalle.Rows)
				{
					descrip   = row["codigo"].ToString().Trim()+" "+row["descrip"].ToString();
					cant      = Convert.ToDecimal(row["cant"]).ToString("###0.000");
					cant      = cant.Replace(",","").Replace(".","");
					cant      = cant.PadLeft(8,'0');
					precio    = Convert.ToDecimal(row["precio"]).ToString("#####0.00");
					precio    = precio.Replace(",","").Replace(".","");
					precio    = precio.PadLeft(10,'0');
					talla     = row["talla"].ToString();
					color     = row["color"].ToString();
					seriales  = row["seriales"].ToString();
					notas     = row["notas"].ToString();
					i         = Convert.ToInt16(row["baseiva"]);

					cadena    = "d"+tipotasa[i]+precio+cant+descrip;
					respuesta = SendCmd(ref status,ref error,cadena);

					if (!string.IsNullOrEmpty(talla))
					{
		     			cadena    = "@ "+talla+" / "+color;
		     			respuesta = SendCmd(ref status,ref error,cadena);
					}

					if (!string.IsNullOrEmpty(seriales))
					{
		     			cadena    = "@ S/N:"+seriales;
		     			respuesta = SendCmd(ref status,ref error,cadena);
					}
				}

				cadena    = "f01";
				respuesta = SendCmd(ref status,ref error,cadena);

				ok        = evalua_errores(respuesta);

				CloseFpctrl();
		    }

			return(ok);
		}

		public static bool GeneraDocumentoNoFiscal(string puerto,DataTable dtNOFiscal)
		{
			int    respuesta;
			long   status = 0;
			long   error  = 0;
			string cadena;
			bool   ok     = false;

			if (abrir_puerto(puerto) )
		    {

				foreach (DataRow row in dtNOFiscal.Rows)
				{
					cadena    = "800"+row["linea"].ToString().Trim();
	     			respuesta = SendCmd(ref status,ref error,cadena);
				}

				cadena    = "810";
     			respuesta = SendCmd(ref status,ref error,cadena);

     			ok        = evalua_errores(respuesta);

     			CloseFpctrl();
		    }

			return(ok);
		}

		public static string NumSerial(string codfiscal,string puerto,bool leerMemoria)
		{
			string serial;

			serial = LeerStatusS1(puerto,leerMemoria,66,10);

			if (codfiscal == "BIXOLON350" || codfiscal == "DASCOM230")
			{
				serial = LeerStatusS1(puerto,leerMemoria,103,13);
			}

			return(serial);
		}

		public static string UltNumFactura(string codfiscal,string puerto,bool leerMemoria)
		{
			string numero;

			numero = LeerStatusS1(puerto,leerMemoria,21,8);

			return(numero);
		}

		public static string UltNumNotaCredito(string codfiscal,string puerto,bool leerMemoria)
		{
			string numero;

			numero = LeerStatusS1(puerto,leerMemoria,168,8);
			numero = LeerStatusS1(puerto,leerMemoria,88,8);

			if (codfiscal == "BIXOLON350" || codfiscal == "DASCOM230")
			{
				numero = LeerStatusS1(puerto,leerMemoria,34,8);
			}

			return(numero);
		}

		public static string UltNumRepZ(string codfiscal,string puerto,bool leerMemoria)
		{
			string numero;

			numero = LeerStatusS1(puerto,leerMemoria,47,4);

			if (codfiscal == "BIXOLON350" || codfiscal == "DASCOM230")
			{
				numero = LeerStatusS1(puerto,leerMemoria,73,4);
			}

			return(numero);
		}

		public static string FechaFiscal(string codfiscal,string puerto,bool leerMemoria)
		{
			string fecha;

			fecha = LeerStatusS1(puerto,leerMemoria,82,6);

			if (codfiscal == "BIXOLON350" || codfiscal == "DASCOM230")
			{
				fecha = LeerStatusS1(puerto,leerMemoria,122,6);
			}

			return(fecha);
		}

		private static string LeerStatusS1(string puerto,bool leerMemoria,int pos,int lon)
		{
			int    respuesta;
			long   status = 0;
			long   error  = 0;
			string cadena, file = "", dato = "";
			bool   ok     = false;

			if ( abrir_puerto(puerto) )
		    {
     			cadena    = "S1";
     			file      = "tfhka_s1";
     			if (leerMemoria) respuesta = UploadStatusCmd(ref status,ref error,cadena,file);

     			CloseFpctrl();

     			FileStream   stream  = new FileStream(file,FileMode.Open,FileAccess.Read);
				StreamReader reader  = new StreamReader(stream);
				string       linea   = (reader.Peek() > -1 ? reader.ReadLine() : "" );

				try
				{
					dato = linea.Substring(pos,lon);
				}
				catch
				{
					dato = "";
				}

				reader.Close();
				reader.Dispose();
				stream.Close();
				stream.Dispose();
			}

			return(dato);
		}

		private static string LeerStatusS2(bool leerMemoria,int pos,int lon)
		{
			int    respuesta;
			long   status = 0;
			long   error  = 0;
			string cadena, file = "", dato = "";
			bool   ok     = false;

     		cadena    = "S2";
     		file      = "tfhka_s2";
     		if (leerMemoria) respuesta = UploadStatusCmd(ref status,ref error,cadena,file);

     		FileStream   stream  = new FileStream(file,FileMode.Open,FileAccess.Read);
			StreamReader reader  = new StreamReader(stream);
			string       linea   = (reader.Peek() > -1 ? reader.ReadLine() : "" );

			try
			{
				dato = linea.Substring(pos,lon);
			}
			catch
			{
				dato = "";
			}

			reader.Close();
			reader.Dispose();
			stream.Close();
			stream.Dispose();

			return(dato);
		}

		public static string LeerUltimoRepX(string puerto,bool leerMemoria,int pos,int lon)
		{
			int    respuesta;
			long   status = 0;
			long   error  = 0;
			string cadena, file = "", dato = "";
			bool   ok     = false;

			if ( abrir_puerto(puerto) )
		    {
     			cadena    = "U0X";
     			file      = "tfhka_u0x";
     			if (leerMemoria) respuesta = UploadReportCmd(ref status,ref error,cadena,file);

     			CloseFpctrl();

     			FileStream   stream  = new FileStream(file,FileMode.Open,FileAccess.Read);
				StreamReader reader  = new StreamReader(stream);
				string       linea   = (reader.Peek() > -1 ? reader.ReadLine() : "" );

				lon       = (lon == 0 ? linea.Length : lon);

				try
				{
					dato = linea.Substring(pos,lon);
				}
				catch
				{
					dato = "";
				}

				reader.Close();
				reader.Dispose();
				stream.Close();
				stream.Dispose();
			}

			return(dato);
		}

		public static bool ReporteZ(string puerto)
		{
			int    respuesta;
			long   status = 0;
			long   error  = 0;
			string cadena;
			bool   ok     = false;

			if ( abrir_puerto(puerto) )
		    {
     			cadena    = "I0Z";
     			respuesta = SendCmd(ref status,ref error,cadena);
     			ok        = evalua_errores(respuesta);
     			CloseFpctrl();
			}

			return(ok);
		}

		public static bool ReporteX(string puerto)
		{
			int    respuesta;
			long   status = 0;
			long   error  = 0;
			string cadena;
			bool   ok     = false;

			if ( abrir_puerto(puerto) )
		    {
     			cadena    = "I0X";
     			respuesta = SendCmd(ref status,ref error,cadena);
     			ok        = evalua_errores(respuesta);
     			CloseFpctrl();
			}

			return(ok);
		}

		public static bool Reimprimir(string puerto,int tipodoc,bool porfecha,
		                              string desde,string hasta)
		{
			int    respuesta;
			long   status = 0;
			long   error  = 0;
			string cadena;
			bool   ok     = false;
			string[] tipo = {"F","C","Z"};

			if ( abrir_puerto(puerto) )
		    {
				desde     = desde.Trim().PadLeft(7,'0');
				hasta     = hasta.Trim().PadLeft(7,'0');

				cadena    = "R"+(porfecha ? tipo[(tipodoc-1)].ToLower() : tipo[(tipodoc-1)])+desde+hasta;
     			respuesta = SendCmd(ref status,ref error,cadena);
     			ok        = evalua_errores(respuesta);
     			CloseFpctrl();
			}

			return(ok);
		}

		public static bool ExtraerRepZ(string puerto)
		{
			bool     ok    = false;
			string   linea, repznum, ultfactnum, ultncnum;
			decimal  exento, ncexento, base1, base2, base3, ncbase1, ncbase2, ncbase3, iva1, iva2, iva3, nciva1, nciva2, nciva3;
			DateTime ultfactfecha, ultncfecha;

			linea = LeerUltimoRepX(puerto,true,0,0);

			if (linea.Length > 0)
			{
				repznum      = linea.Substring(00,04);
				repznum      = (Convert.ToInt16(repznum) + 1).ToString().PadLeft(4,'0');
				exento       = (Convert.ToDecimal(linea.Substring(28,10)) / 100);
				base1        = (Convert.ToDecimal(linea.Substring(38,10)) / 100);
				iva1         = (Convert.ToDecimal(linea.Substring(48,10)) / 100);
				base2        = (Convert.ToDecimal(linea.Substring(58,10)) / 100);
				iva2         = (Convert.ToDecimal(linea.Substring(68,10)) / 100);
				base3        = (Convert.ToDecimal(linea.Substring(78,10)) / 100);
				iva3         = (Convert.ToDecimal(linea.Substring(88,10)) / 100);
				ncexento     = (Convert.ToDecimal(linea.Substring(98,10)) / 100);
				ncbase1      = (Convert.ToDecimal(linea.Substring(108,10)) / 100);
				nciva1       = (Convert.ToDecimal(linea.Substring(118,10)) / 100);
				ncbase2      = (Convert.ToDecimal(linea.Substring(128,10)) / 100);
				nciva2       = (Convert.ToDecimal(linea.Substring(138,10)) / 100);
				ncbase3      = (Convert.ToDecimal(linea.Substring(148,10)) / 100);
				nciva3       = (Convert.ToDecimal(linea.Substring(158,10)) / 100);
				ultfactnum   = linea.Substring(10,08);
				ultfactfecha = DateTime.Now;
				ultncnum     = linea.Substring(168,08);
				ultncfecha   = DateTime.Now;

				ok           = fiscal.RepZtoXML(repznum,exento,base1,iva1,base2,iva2,
				                                base3,iva3,ncexento,ncbase1,nciva1,
				                                ncbase2,nciva2,ncbase3,nciva3,
				                                ultfactnum,ultfactfecha,ultncnum,ultncfecha);
			}

			return(ok);
		}

		public static bool abrir_gaveta(string puerto)
		{
			int    respuesta;
			long   status = 0;
			long   error  = 0;
			string cadena;
			bool   ok     = false;

			if ( abrir_puerto(puerto) )
		    {
				cadena    = "w";
				respuesta = SendCmd(ref status,ref error,cadena);
				ok        = evalua_errores(respuesta);

			    CloseFpctrl();
		    }

			return(ok);
		}

		public static bool abrir_puerto(string puerto)
		{
			int    respuesta;
			bool   ok;

			CloseFpctrl();

			respuesta = OpenFpctrl(puerto);
			ok        = evalua_errores(respuesta);

			return(ok);
		}

		public static bool evalua_errores(int respuesta)
		{
			bool ok = true;

			if (respuesta != 1)
			{
				MessageBox.Show("Error impresora fiscal");
				ok = false;
			}

			return(ok);
		}
	}
}