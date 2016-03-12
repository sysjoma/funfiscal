/*
 */
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Data;
using System.Threading;

namespace funfiscal
{

	public class bemafi32
	{
		public bemafi32()
		{
		}
		
		#region DECLARACIÓN DE LAS FUNCIONES DE LA BEMAFI32.DLL   

		#region Funciones de Inicialización     
		[DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_ProgramaAlicuota(string Alicuota, int ICMS_ISS);     
		[DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_ProgramaRedondeo();   
		[DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_ProgramaTruncamiento();   
		#endregion   

		#region Funciones del Cupon Fiscal   
		[DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_AbreComprobanteDeVenta(string RIF, string Nombre);   
		[DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_AbreComprobanteDeVentaEx(string RIF, string Nombre, string Direccion);  
		[DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_VendeArticulo(string Codigo, string Descripcion, string Alicuota, string TipoCantidad, string Cantidad, int CasasDecimales, string ValorUnitario, string TipoDescuento, string Descuento);     
		[DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_AnulaArticuloAnterior();    
		[DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_AnulaCupon();     
		[DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_CierraCupon(string FormaPago, string IncrementoDescuento, string TipoIncrementoDescuento, string ValorIncrementoDescuento, string ValorPago, string Mensaje);   
		[DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_IniciaCierreCupon(string IncrementoDescuento, string TipoIncrementoDescuento, string ValorIncrementoDescuento);		
		[DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_EfectuaFormaPago(string FormaPago, string ValorFormaPago);
		[DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_EstenderDescripcionArticulo(string Descripcion);		
		[DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_FinalizarCierreCupon(string Mensaje);   
		[DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_DevolucionArticulo(string Codigo, string Descripcion, string Alicuota, string TipoCantidad, string Cantidad, int CasasDecimales, string ValorUnit, string TipoDescuento, string ValorDesc);
		[DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_AbreNotaDeCredito(string Nombre, string NumeroSerie, string RIF, string Dia, string Mes, string Ano, string Hora, string Minuto, string Segundo, string COO, string MsjPromocional);   
		#endregion   

		#region Funciones de los Informes Fiscales   
		[DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_LecturaX();   
		[DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_LecturaXSerial();   
		[DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_ReduccionZ(string Fecha, string Hora);   
		[DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_InformeGerencial(string Texto);   
		[DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_CierraInformeGerencial();   
		#endregion   

		#region Funciones de las Operaciones No Fiscales   
		[DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_RecebimientoNoFiscal(string IndiceTotalizador, string Valor, string FormaPago);   
		[DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_AbreComprobanteNoFiscalVinculado(string FormaPago, string Valor, string NumeroCupon);   
		[DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_ImprimeComprobanteNoFiscalVinculado(string Texto);   
		[DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_CierraComprobanteNoFiscalVinculado();   
		[DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_Sangria(string Valor);   
		[DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_Provision(string Valor, string FormaPago);   
		#endregion   

		#region Funciones de Informaciones de la Impresora   
		[DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_Agregado([MarshalAs(UnmanagedType.VBByRefStr)] ref string ValorIncrementos);   
		[DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_Cancelamientos([MarshalAs(UnmanagedType.VBByRefStr)] ref string ValorCancelamientos);   
		[DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_ContadorNotaDeCreditoMFD([MarshalAs(UnmanagedType.VBByRefStr)] ref string Veces);
		[DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_DatosUltimaReduccion([MarshalAs(UnmanagedType.VBByRefStr)] ref string DatosReduccion);
		[DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_Descuentos([MarshalAs(UnmanagedType.VBByRefStr)] ref string ValorDescuentos);
		[DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_FechaHoraImpresora([MarshalAs(UnmanagedType.VBByRefStr)] ref string Fecha, [MarshalAs(UnmanagedType.VBByRefStr)] ref string Hora);
		[DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_NumeroCuponesAnulados([MarshalAs(UnmanagedType.VBByRefStr)] ref string NumeroCancelamientos);
        [DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_RetornoAlicuotas([MarshalAs(UnmanagedType.VBByRefStr)] ref string Alicuotas);
        [DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_NumeroComprobanteFiscal([MarshalAs(UnmanagedType.VBByRefStr)] ref string Numero);
        [DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_NumeroReducciones([MarshalAs(UnmanagedType.VBByRefStr)] ref string Reducciones);
        [DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_NumeroSerie([MarshalAs(UnmanagedType.VBByRefStr)] ref string NumeroSerie);
        [DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_SubTotal([MarshalAs(UnmanagedType.VBByRefStr)] ref string SubTotal);
		#endregion   

		#region Funciones de Autenticación y Gaveta de Efectivo   
		[DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_AccionaGaveta();   
		[DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_Autenticacion();   
		[DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_ProgramaCaracterAutenticacion(string Parametros);   
		[DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_VerificaEstadoGaveta(out int EstadoGaveta);   
		#endregion   

		#region Otras Funciones    
		[DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_AbrePuertaSerial();     
		[DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_CierraPuertaSerial();     
		[DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_RetornoImpresora(ref int ACK, ref int ST1, ref int ST2);   
	  
		#endregion 

		#endregion


		public static bool GeneraFactura(string referencia,
		                                 string rifoci,string nomcli,string dircli,
		                                 string tlfcli,string vendedor,string condipago,
		                                 decimal totaldoc,DateTime fechavence,
		                                 DataTable dtDetalle,DataTable dtPagos,
		                                 string checkin,string huesped,string habino,
		                                 string placa,string cajero,string estacion,
		                                 string mensajePie)
		{
			int     respuesta;
			bool    ok         = false;
			string  codigo, descrip, cant, precio, iva, talla, color, seriales, notas;

			abrir_gaveta();

			rifoci    = rifoci.Replace("-","");
			rifoci    = rifoci.Substring(0,(rifoci.Length<18?rifoci.Length:18)).Trim();
			nomcli    = nomcli.Substring(0,(nomcli.Length<41?nomcli.Length:41)).Trim();
			dircli    = dircli.Substring(0,(dircli.Length<133?dircli.Length:133)).Trim();

			respuesta = Bematech_FI_AbreComprobanteDeVentaEx(rifoci,nomcli,dircli);

			foreach (DataRow row in dtDetalle.Rows)
			{
				codigo     = row["codigo"].ToString().Trim();
				descrip    = row["descrip"].ToString().Trim();
				codigo     = codigo.Substring(0,(codigo.Length<13?codigo.Length:13));
				descrip    = descrip.Substring(0,(descrip.Length<29?descrip.Length:29));
				cant       = Convert.ToDecimal(row["cant"]).ToString("###0.000");
				cant       = cant.Replace(",",".").Trim();
				precio     = Convert.ToDecimal(row["precio"]).ToString("#####0.00");
				precio     = precio.Replace(",",".").Trim();
				iva        = Convert.ToDecimal(row["tasaiva"]).ToString("00.00");
				iva        = iva.Replace(",",".").Trim();
				iva        = (iva == "00.00" ? "II" : iva);
				talla      = row["talla"].ToString();
				color      = row["color"].ToString();
				seriales   = row["seriales"].ToString();
				notas      = row["notas"].ToString();

				if (!string.IsNullOrEmpty(talla))
				{
					respuesta  = Bematech_FI_EstenderDescripcionArticulo(talla+" / "+color);
				}

				if (!string.IsNullOrEmpty(seriales))
				{
					respuesta  = Bematech_FI_EstenderDescripcionArticulo("S/N:"+seriales);
				}

				respuesta = Bematech_FI_VendeArticulo(codigo,descrip,iva,"F",cant,2,precio,"%","0");
			}

			respuesta = Bematech_FI_IniciaCierreCupon("D","%","0");

			//respuesta = Bematech_FI_SubTotal(ref sutot);

			//respuesta = Bematech_FI_EfectuaFormaPago("PAGO",sutot);

			notas = ("REF. "+referencia+" / "+cajero+" / "+estacion).PadRight(48);

			if (!string.IsNullOrEmpty(huesped))
			{
				notas = (notas+" HUESPED: "+huesped);
			}

			if (!string.IsNullOrEmpty(habino))
			{
				notas = (notas+" HAB. No: "+habino);
			}

			notas     = (notas+mensajePie).PadRight(384);

			respuesta = Bematech_FI_FinalizarCierreCupon(notas);

			ok = evalua_errores(respuesta);

			return(ok);
		}

		public static bool GeneraNotaCredito(string referencia,string rifoci,string nomcli,string dircli,string tlfcli,string vendedor,DataTable dtDetalle,string checkin,string huesped,string habino,string placa,string cajero,string estacion,string facturaNC,DateTime fechaFactura,string serialFiscal)
		{
			int      respuesta;
			bool     ok         = false;
			string   codigo, descrip, cant, precio, iva, talla, color, seriales, notas,
					 dia, mes, amo, hora, min, seg;
			DateTime fecha = DateTime.Now;		

			if (false)
			{
				abrir_gaveta();
			}

			rifoci       = rifoci.Replace("-","");
			rifoci       = rifoci.Substring(0,(rifoci.Length<18?rifoci.Length:18)).Trim();
			nomcli       = nomcli.Substring(0,(nomcli.Length<41?nomcli.Length:41)).Trim();
			dircli       = dircli.Substring(0,(dircli.Length<133?dircli.Length:133)).Trim();
			serialFiscal = serialFiscal.PadRight(15);
			dia          = fecha.ToString("dd");
			mes          = fecha.ToString("MM");
			amo          = fecha.ToString("yy");
			hora         = fecha.ToString("hh");
			min          = fecha.ToString("mm");
			seg          = fecha.ToString("ss");

			respuesta    = Bematech_FI_AnulaCupon();

			respuesta    = Bematech_FI_AbreNotaDeCredito(nomcli,serialFiscal,rifoci,dia,
			                                             mes,amo,hora,min,seg,"123456","");

			foreach (DataRow row in dtDetalle.Rows)
			{
				codigo     = row["codigo"].ToString().Trim();
				descrip    = row["descrip"].ToString().Trim();
				codigo     = codigo.Substring(0,(codigo.Length<13?codigo.Length:13));
				descrip    = descrip.Substring(0,(descrip.Length<29?descrip.Length:29));
				cant       = Convert.ToDecimal(row["cant"]).ToString("###0.000");
				cant       = cant.Replace(",",".").Trim();
				precio     = Convert.ToDecimal(row["precio"]).ToString("#####0.00");
				precio     = precio.Replace(",",".").Trim();
				iva        = Convert.ToDecimal(row["tasaiva"]).ToString("00.00");
				iva        = iva.Replace(",",".").Trim();
				iva        = (iva == "00.00" ? "II" : iva);
				talla      = row["talla"].ToString();
				color      = row["color"].ToString();
				seriales   = row["seriales"].ToString();
				notas      = row["notas"].ToString();

				if (!string.IsNullOrEmpty(talla))
				{
					respuesta  = Bematech_FI_EstenderDescripcionArticulo(talla+" / "+color);
				}

				if (!string.IsNullOrEmpty(seriales))
				{
					respuesta  = Bematech_FI_EstenderDescripcionArticulo("S/N:"+seriales);
				}

				respuesta = Bematech_FI_VendeArticulo(codigo,descrip,iva,"F",cant,2,precio,"%","0");
			}

			respuesta = Bematech_FI_IniciaCierreCupon("D","%","0");

			notas = ("REF. "+referencia+" / "+cajero+" / "+estacion).PadRight(48);

			if (!string.IsNullOrEmpty(huesped))
			{
				notas = (notas+" HUESPED: "+huesped);
			}

			if (!string.IsNullOrEmpty(habino))
			{
				notas = (notas+" HAB. No: "+habino);
			}

			respuesta = Bematech_FI_FinalizarCierreCupon(notas);

			ok = evalua_errores(respuesta);

			return(ok);
		}

		public static bool GeneraDocumentoNoFiscal(DataTable dtNOFiscal)
		{
			int  respuesta;
			bool ok        = false;

			foreach (DataRow row in dtNOFiscal.Rows)
			{
				respuesta = Bematech_FI_InformeGerencial(row["linea"].ToString().Trim().PadRight(48,' '));
			}

			respuesta = Bematech_FI_CierraInformeGerencial();

			ok = evalua_errores(respuesta);

			return(ok);
		}

		public static string NumSerial()
		{
			int    respuesta;
			string serial    = new string(' ', 15);
			bool   ok        = false;

			respuesta = Bematech_FI_NumeroSerie(ref serial);

			ok = evalua_errores(respuesta);

			return(serial);
		}

		public static string UltNumFactura()
		{
			int    respuesta;
			string numero    = new string(' ', 6);
			bool   ok        = false;

			respuesta = Bematech_FI_NumeroComprobanteFiscal(ref numero);

			ok = evalua_errores(respuesta);

			return(numero);
		}

		public static string UltNumNotaCredito()
		{
			int    respuesta;
			string numero    = new string(' ', 6);
			bool   ok        = false;

			respuesta = Bematech_FI_ContadorNotaDeCreditoMFD(ref numero);

			return(numero);
		}

		public static string UltNumRepZ()
		{
			int    respuesta;
			string numero    = new string(' ', 4);
			bool   ok        = false;

			respuesta = Bematech_FI_NumeroReducciones(ref numero);

			ok = evalua_errores(respuesta);

			return(numero);
		}

		public static string FechaFiscal()
		{
			int    respuesta;
			string fecha     = new string(' ', 6);
			string hora      = new string(' ', 6);
			bool   ok        = false;

			respuesta = Bematech_FI_FechaHoraImpresora(ref fecha,ref hora);

			ok = evalua_errores(respuesta);

			return(fecha);
		}

		public static bool ReporteZ()
		{
			int  respuesta;
			bool ok = false;

			respuesta = Bematech_FI_ReduccionZ("","");

			ok = evalua_errores(respuesta);

			return(ok);
		}

		public static bool ReporteX()
		{
			int  respuesta;
			bool ok = false;

			respuesta = Bematech_FI_LecturaX();

			ok = evalua_errores(respuesta);

			return(ok);
		}

		public static bool CancelaDoc()
		{
			int  respuesta;
			bool ok = false;

			respuesta = Bematech_FI_AnulaCupon();

			ok = evalua_errores(respuesta);

			return(ok);
		}

		public static bool abrir_gaveta()
		{
			int  respuesta;
			bool ok = false;

			respuesta = Bematech_FI_AccionaGaveta();

			ok = evalua_errores(respuesta);

			return(ok);
		}

		public static bool evalua_errores(int respuesta)
		{
			bool   ok  = (respuesta == 1);
            string msg = "";

			switch(respuesta)
			{
				case  0: 
					msg = "Error de Comunicación !";
					break;
				case -1: 
					msg = "Error de ejecución en la función. Verifique!";
					break;
				case -2: 
					msg = "Parámetro Inválido !";
					break;
				case -3: 
					msg = "Alicuota no programada !";
					break;
				case -4: 
					msg = "Archivo BemaFI32.INI no encontrado. Verifique!";
					break;
				case -5: 
					msg = "Error al abrir el puerto de comunicación";
					break;
				case -6: 
					msg = "Impresora apagada o desconectada";
					break;
				case -7: 
					msg = "Banco no registrado en el Archivo BemaFI32.ini";
					break;
				case -8: 
					msg = "Error al crear o Grabar en el archivo Retorno.txt o Status.txt";
					break;
				case -18: 
					msg = "No fué posíble abrir el archivo INTPOS.001!";
					break;
				case -19: 
					msg = "Parámetros diferentes!";
					break;
				case -20: 
					msg = "Transación anulada por el operador!";
					break;
				case -21: 
					msg = "La transación no fué aprobada!";
					break;
				case -22: 
					msg = "No fué posible finalizar la impresión!";
					break;
				case -23:
					msg = "No fué posible finalizar la operación!";
					break;
				case -24: 
					msg = "No fué posible finalizar la operación!";
					break;
				case -25:
					msg = "Totalizador no fiscal no programado";
					break;
				case -26:
					msg = "Transación ya efectuada!";
					break;
				case -27:
					msg = "Status de la impresora diferente de 6,0,0 (ACK, ST1 y ST2)";
					break;
				case -28:
					msg = "No hay informaciones para imprimir!";
					break;
			}

			if (msg.Length > 0)
			{
				MessageBox.Show(msg);
				ok = false;
			}

			return(ok);
		}

	}
}
