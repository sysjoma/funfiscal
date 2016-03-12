/*
 */
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Diagnostics;

namespace funfiscal
{

	public class hasar
	{

		const char c28 = (char)28;

		private static string	FS       = c28.ToString();

		private static int		handler;

		private static string	respuesta = "";

		public hasar()
		{
		}
		
		#region DECLARACIÓN DE LAS FUNCIONES DE LA WINFIS32.DLL

//		[DllImport("winfis32.dll")] public static extern void	Abort(int handler);
//		[DllImport("winfis32.dll")] public static extern void 	SetKeepAliveHandlerStdCall(int Ptr);
//		[DllImport("winfis32.dll")] public static extern int 	VersionDLLFiscal();
//		[DllImport("winfis32.dll")] public static extern int 	OpenComFiscal(int puerto, int mode);
//		[DllImport("winfis32.dll")] public static extern int 	ReOpenComFiscal(int puerto);
//		[DllImport("winfis32.dll")] public static extern int 	OpenTcpFiscal(string hotsname, int socket, double miliseg, int mode);
//		[DllImport("winfis32.dll")] public static extern void 	CloseComFiscal(int handler);
//		[DllImport("winfis32.dll")] public static extern int 	InitFiscal(int handler);
//		[DllImport("winfis32.dll")] public static extern int 	CambiarVelocidad(int handler, int NewSpeed);
//		[DllImport("winfis32.dll")] public static extern void 	BusyWaitingMode(int mode);
//		[DllImport("winfis32.dll")] public static extern void 	ProtocolMode(int mode);
//		[DllImport("winfis32.dll")] public static extern int 	SetModoEpson(bool epson);
//		[DllImport("winfis32.dll")] public static extern int 	SearchPrn(int handler);
//		[DllImport("winfis32.dll")] public static extern int 	MandaPaqueteFiscal(int handler, string Buffer);
//		[DllImport("winfis32.dll")] public static extern int 	UltimaRespuesta(int handler, ref string buffer);
//		[DllImport("winfis32.dll")] public static extern int 	UltimoStatus(int handlre, int FiscalStatus, int PrinterStatus);
//		[DllImport("winfis32.dll")] public static extern int 	SetCmdRetries(int cat);
//		[DllImport("winfis32.dll")] public static extern int 	SetSndRetries(int cat);
//		[DllImport("winfis32.dll")] public static extern int 	SetRcvRetries(int cat);
//		[DllImport("winfis32.dll")] public static extern int 	ObtenerNumeroDePaquetes(int handler, int Paqsend, int Paqrec, string Idcmd);

		#endregion


		public static bool GeneraFactura(string puerto,string referencia,
		                                 string rifoci,string nomcli,string dircli,
		                                 string tlfcli,string vendedor,string condipago,
		                                 decimal totaldoc,DateTime fechavence,
		                                 DataTable dtDetalle,DataTable dtPagos,
		                                 string checkin,string huesped,string habino,
		                                 string placa,string cajero,string estacion,
		                                 string mensajePie)
		{
			string       archivo = "facturahasar.txt", comando, codigo, descrip, cant, precio, 
						 iva, talla, color, seriales, notas;
			StreamWriter writer  = File.CreateText(archivo);
			bool         ok;


			int          lon = mensajePie.Length, i = 0, m, y = 1;

			while (i < lon)
			{
				m       = ((lon-i) > 46 ? 46 : (lon-i));
				comando = "^"+FS+y.ToString()+FS+mensajePie.Substring(i,m);
				writer.WriteLine(comando);
				i += 46;
				y++;
			}


			rifoci    = rifoci.Substring(0,(rifoci.Length<30?rifoci.Length:30)).Trim();
			nomcli    = nomcli.Substring(0,(nomcli.Length<80?nomcli.Length:80)).Trim();
			dircli    = dircli.Trim();

			char c127 = (char)127;
			comando   = "@"+FS+nomcli+FS+rifoci+FS+referencia+FS+"ABC123"+FS+
						DateTime.Now.ToString("yyMMdd")+FS+DateTime.Now.ToString("hhmmss")+FS+
						"A"+FS+c127.ToString()+FS+c127.ToString();
			writer.WriteLine(comando);

			comando   = "A"+FS+"REF. "+referencia+" / "+cajero+" / "+estacion;
			writer.WriteLine(comando);

			comando   = "A"+FS+"VEND."+vendedor;
			writer.WriteLine(comando);

			if (!string.IsNullOrEmpty(huesped))
			{
     			comando = "A"+FS+"HUESPED "+huesped;
				writer.WriteLine(comando);
			}

			if (!string.IsNullOrEmpty(habino))
			{
     			comando = "A"+FS+"HAB. No "+habino;
				writer.WriteLine(comando);
			}

			foreach (DataRow row in dtDetalle.Rows)
			{
				codigo   = row["codigo"].ToString().Trim();
				descrip  = row["descrip"].ToString().Trim();
				cant     = Convert.ToDecimal(row["cant"]).ToString("###0.000");
				cant     = cant.Replace(".",",");
				precio   = Convert.ToDecimal(row["precio"]).ToString("#####0.00");
				precio   = precio.Replace(".",",");
				iva      = Convert.ToDecimal(row["tasaiva"]).ToString("00.00");
				iva      = iva.Replace(".",",").Trim();
				talla    = row["talla"].ToString();
				color    = row["color"].ToString();
				seriales = row["seriales"].ToString();
				notas    = row["notas"].ToString();

	    		comando  = "B"+FS+descrip+FS+cant+FS+precio+FS+iva+FS+"M"+FS+codigo;
				writer.WriteLine(comando);
			}

   			comando   = "D"+FS+"Efectivo"+FS+"0.00"+FS+"T"+FS+"1";
			writer.WriteLine(comando);

     		comando   = "E"+FS;
     		writer.WriteLine(comando);

			writer.Close();
			writer.Dispose();

			abrir_gaveta(puerto);

			ok = EjecutarWSpooler(puerto,"",archivo);

			return(ok);
		}

		public static bool GeneraNotaCredito(string puerto,string referencia,string rifoci,
		                                     string nomcli,string dircli,string tlfcli,
		                                     string vendedor,DataTable dtDetalle,
		                                     string checkin,string huesped,string habino,
		                                     string placa,string cajero,string estacion,
		                                     string facturaNC,DateTime fechaFactura,string serialFiscal)
		{
			string       archivo = "notacreditohasar.txt", comando, codigo, descrip, cant, precio, 
						 iva, talla, color, seriales, notas;
			StreamWriter writer  = File.CreateText(archivo);
			bool         ok;

			rifoci    = rifoci.Substring(0,(rifoci.Length<30?rifoci.Length:30)).Trim();
			nomcli    = nomcli.Substring(0,(nomcli.Length<80?nomcli.Length:80)).Trim();
			dircli    = dircli.Trim();

			char c127 = (char)127;
			comando   = "@"+FS+nomcli+FS+rifoci+FS+facturaNC+FS+serialFiscal+FS+
						fechaFactura.ToString("yyMMdd")+FS+fechaFactura.ToString("hhmmss")+FS+
						"D"+FS+c127.ToString()+FS+c127.ToString();
			writer.WriteLine(comando);

			comando   = "A"+FS+"REF. "+referencia+" / "+cajero+" / "+estacion;
			writer.WriteLine(comando);

			if (!string.IsNullOrEmpty(huesped))
			{
     			comando = "A"+FS+"HUESPED "+huesped;
				writer.WriteLine(comando);
			}

			if (!string.IsNullOrEmpty(habino))
			{
     			comando = "A"+FS+"HAB. No "+habino;
				writer.WriteLine(comando);
			}

			foreach (DataRow row in dtDetalle.Rows)
			{
				codigo   = row["codigo"].ToString().Trim();
				descrip  = row["descrip"].ToString().Trim();
				cant     = Convert.ToDecimal(row["cant"]).ToString("###0.000");
				cant     = cant.Replace(".",",");
				precio   = Convert.ToDecimal(row["precio"]).ToString("#####0.00");
				precio   = precio.Replace(".",",");
				iva      = Convert.ToDecimal(row["tasaiva"]).ToString("00.00");
				iva      = iva.Replace(".",",").Trim();
				talla    = row["talla"].ToString();
				color    = row["color"].ToString();
				seriales = row["seriales"].ToString();
				notas    = row["notas"].ToString();

	    		comando  = "B"+FS+descrip+FS+cant+FS+precio+FS+iva+FS+"M"+FS+codigo;
				writer.WriteLine(comando);
			}

   			comando   = "D"+FS+"Efectivo"+FS+"0.00"+FS+"T"+FS+"1";
			writer.WriteLine(comando);

     		comando   = "E"+FS;
     		writer.WriteLine(comando);

			writer.Close();
			writer.Dispose();

			abrir_gaveta(puerto);

			ok = EjecutarWSpooler(puerto,"",archivo);

			return(ok);
		}

		public static bool GeneraDocumentoNoFiscal(string puerto,DataTable dtNOFiscal)
		{
			string       archivo = "nofiscalhasar.txt", comando;
			StreamWriter writer  = File.CreateText(archivo);
			bool         ok;

			comando = "H"+FS+"O";
			writer.WriteLine(comando);

			foreach (DataRow row in dtNOFiscal.Rows)
			{
				comando = "I"+FS+row["linea"].ToString().PadRight(42).Trim();
				writer.WriteLine(comando);
			}

     		comando = "J"+FS;
     		writer.WriteLine(comando);

			writer.Close();
			writer.Dispose();

			ok = EjecutarWSpooler(puerto,"",archivo);

			return(ok);
		}

		public static string NumSerial(string puerto)
		{
			string comando, serial = "";
			int    d, h;

			char c128 = (char)128;
			comando   = c128.ToString()+" ";
			comando   = "Ç";

			EjecutarWSpooler(puerto,comando);

			if (respuesta != "")
			{
				try
				{
					d      = posicion(respuesta,"|",2);
					h      = posicion(respuesta,"|",3);
					serial = respuesta.Substring((d+1),(h-d-1));
				}
				catch
				{
					serial = "";
				}
			}

			return(serial);
		}

		public static string FechaFiscal(string puerto)
		{
			string comando, fecha = "";
			int    d, h;

			comando = "Y";

			EjecutarWSpooler(puerto,comando);

			if (respuesta != "")
			{
				try
				{
					d     = posicion(respuesta,"|",2);
					h     = posicion(respuesta,"|",3);
					fecha = respuesta.Substring((d+1),(h-d-1));
					fecha = fecha.Substring(4,2)+fecha.Substring(2,2)+fecha.Substring(0,2);
				}
				catch
				{
					fecha = "";
				}
			}

			return(fecha);
		}

		public static string LeerMemoria(string puerto,bool leerMemoria,int pos)
		{
			string comando, cadena = "";
			int    d, h;

			comando = "g";
			comando = "*";

			if (leerMemoria) EjecutarWSpooler(puerto,comando);

			if (respuesta != "")
			{
				try
				{
					d      = posicion(respuesta,"|",pos);
					h      = posicion(respuesta,"|",(pos+1));
					cadena = respuesta.Substring((d+1),(h-d-1));
				}
				catch
				{
					cadena = "";
				}
			}

			return(cadena);
		}

		public static bool ReporteZ(string puerto)
		{
			string comando;
			bool   ok;

			comando = "9"+FS+"Z"+FS+" ";

			ok = EjecutarWSpooler(puerto,comando);

			return(ok);
		}

		public static bool ReporteX(string puerto)
		{
			string comando;
			bool   ok;

			comando = "9"+FS+"X"+FS+" ";

			ok = EjecutarWSpooler(puerto,comando);

			return(ok);
		}

		public static bool CancelaDoc()
		{
			int    respuesta;
			string comando;
			bool   ok   = false;

//			char id   = (char)152;
//			comando   = id.ToString();
//   			respuesta = MandaPaqueteFiscal(handler,comando);
//   			ok        = evalua_errores(respuesta);

   			return(ok);
		}

		private static bool EjecutarWSpooler(string puerto,string comando,string archivo = "")
		{
			ProcessStartInfo startinfo = new ProcessStartInfo();
			string           arguments, archivoans = ""; 
			bool             ok        = true;

			if (archivo == "")
			{
				arguments  = "-p"+puerto+" -lzt -c "+comando;
			}
			else
			{
				arguments  = "-p"+puerto+" -lzt -f "+archivo;
				archivoans = archivo.Substring(0,archivo.IndexOf("."))+".ans";
			}

			startinfo.CreateNoWindow  = true;
			startinfo.UseShellExecute = false;
			startinfo.WindowStyle     = ProcessWindowStyle.Hidden;
			startinfo.FileName        = "wspooler.exe";
			startinfo.Arguments       = arguments;

			if (File.Exists("spooler.log")) File.Delete("spooler.log");
			if (File.Exists(archivoans))	File.Delete(archivoans);

			using (Process proceso = Process.Start(startinfo))
			{
			     proceso.WaitForExit();
			}

			respuesta = "";

			if (File.Exists("respuesta.ans"))
			{
				FileStream   stream = new FileStream("respuesta.ans",FileMode.Open,FileAccess.Read);
				StreamReader reader = new StreamReader(stream);
				string       linea  = (reader.Peek() > -1 ? reader.ReadLine() : "" );

				respuesta = linea;

				reader.Close();
				reader.Dispose();
				stream.Close();
				stream.Dispose();
			}

			return(ok);
		}

		public static bool abrir_gaveta(string puerto)
		{
			string comando;
			bool   ok;

			comando = "{";

			ok = EjecutarWSpooler(puerto,comando);

			return(ok);
		}

//		public static bool abrir_puerto(int puerto)
//		{
//			int    respuesta;
//			bool   ok;
//
//			respuesta = OpenComFiscal(puerto,1);
//			
//			if (respuesta == -5)
//			{
//				respuesta = ReOpenComFiscal(puerto);
//			}
//
//			ok      = evalua_errores(respuesta);
//
//			handler = respuesta;
//
//			InitFiscal(handler);
//
//			return(ok);
//		}
//
//		public static bool evalua_errores(int respuesta)
//		{
//			bool   ok  = (respuesta >= 0);
//            string msg = "";
//
//			switch(respuesta)
//			{
//				case -1: 
//					msg = "Error general";
//					break;
//				case -2: 
//					msg = "Handler inválido";
//					break;
//				case -3: 
//					msg = "Intento de enviar un comando cuando se estaba procesando";
//					break;
//				case -4: 
//					msg = "Error de comunicaciones";
//					break;
//				case -5: 
//					msg = "Puerto ya abierto";
//					break;
//				case -6: 
//					msg = "No hay memoria";
//					break;
//				case -7: 
//					msg = "El puerto ya estaba abierto";
//					break;
//				case -8: 
//					msg = "La dirección del buffer de respuesta es inválida";
//					break;
//				case -9: 
//					msg = "El comando no finalizó, sino que volvió una respuesta tipo STAT_PRN";
//					break;
//				case -10: 
//					msg = "El proceso en curso fue abortado por el usuario";
//					break;
//				case -11: 
//					msg = "No hay más puertos disponibles";
//					break;
//				case -12: 
//					msg = "Error estableciendo comunicación TCP/IP";
//					break;
//				case -13: 
//					msg = "No se encontró el host";
//					break;
//				case -14:
//					msg = "Error de conexión con el host";
//					break;
//				case -15: 
//					msg = "Se recibió NAK al comando enviado";
//					break;
//			}
//
//			if (msg.Length > 0)
//			{
//				MessageBox.Show(msg);
//				ok = false;
//			}
//
//			return(ok);
//		}

		private static int posicion(string cadena,string unchar,int veces)
		{
			int i, z = 0;
			
			for (i = 0;i < veces;i++)
			{
				z = cadena.IndexOf(unchar,(z+1));
				if (z < 0) break;
			}

			return z;
		}

	}
}
