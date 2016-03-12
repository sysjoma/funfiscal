/*
 */
using System;
using System.Data;
using System.Threading;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Windows.Forms;

namespace funfiscal
{
	public class fiscal
	{
		public  string 		codfiscal;
		public  string 		puerto;

		public  string 		referencia;
		public  string 		rifoci;
		public  string 		nomcli;
		public  string 		dircli;
		public  string 		tlfcli;
		public  string 		vendedor;
		public  string 		condipago;
		public  decimal 	totaldoc;
		public  DateTime 	fechavence;
		public  DataTable 	dtDetalle;
		public  DataTable 	dtPagos;
		public  string 		checkin;
		public  string 		huesped;
		public  string 		habino;
		public  string 		placa;
		public  string 		cajero;
		public  string 		estacion;
		public  string 		mensajePie;
		
		public  string    	FacturaNC;
		public  DateTime 	Fechafactura;
		public  string		serialFiscal;

		public fiscal(string codfiscal,string puerto)
		{
			this.codfiscal  = codfiscal;
			this.puerto     = puerto;
			this.totaldoc   = 0;
			this.mensajePie = "";
		}

		public bool GeneraFactura()
		{
			bool ok = false;

			switch (codfiscal)
			{
				case "BEMATECH":
					ok = bemafi32.GeneraFactura(referencia,rifoci,nomcli,dircli,tlfcli,vendedor,condipago,totaldoc,fechavence,dtDetalle,dtPagos,checkin,huesped,habino,placa,cajero,estacion,mensajePie);
					break;
				case "HASAR":
					ok = hasar.GeneraFactura(puerto,referencia,rifoci,nomcli,dircli,tlfcli,vendedor,condipago,totaldoc,fechavence,dtDetalle,dtPagos,checkin,huesped,habino,placa,cajero,estacion,mensajePie);
					break;
				case "PNP":
					ok = pnp.GeneraFactura(puerto,referencia,rifoci,nomcli,dircli,tlfcli,vendedor,condipago,totaldoc,fechavence,dtDetalle,dtPagos,checkin,huesped,habino,placa,cajero,estacion);
					break;
				case "TFHKA": case "BIXOLON350": case "DASCOM230":
					ok = tfhka.GeneraFactura(codfiscal,puerto,referencia,rifoci,nomcli,dircli,tlfcli,vendedor,condipago,totaldoc,fechavence,dtDetalle,dtPagos,checkin,huesped,habino,placa,cajero,estacion);
					break;
			}

			Thread.Sleep(3000);

			return( ok );
		}

		public bool GeneraNotaCredito()
		{
			bool ok = false;

			switch (codfiscal)
			{
				case "BEMATECH":
					ok = bemafi32.GeneraNotaCredito(referencia,rifoci,nomcli,dircli,tlfcli,vendedor,dtDetalle,checkin,huesped,habino,placa,cajero,estacion,FacturaNC,Fechafactura,serialFiscal);
					break;
				case "HASAR":
					ok = hasar.GeneraNotaCredito(puerto,referencia,rifoci,nomcli,dircli,tlfcli,vendedor,dtDetalle,checkin,huesped,habino,placa,cajero,estacion,FacturaNC,Fechafactura,serialFiscal);
					break;
				case "PNP":
					ok = pnp.GeneraNotaCredito(puerto,referencia,rifoci,nomcli,dircli,tlfcli,vendedor,dtDetalle,checkin,huesped,habino,placa,cajero,estacion,FacturaNC,Fechafactura,serialFiscal);
					break;
				case "TFHKA": case "BIXOLON350": case "DASCOM230":
					ok = tfhka.GeneraNotaCredito(codfiscal,puerto,referencia,rifoci,nomcli,dircli,tlfcli,vendedor,dtDetalle,checkin,huesped,habino,placa,cajero,estacion,FacturaNC,Fechafactura,serialFiscal);
					break;
			}

			Thread.Sleep(3000);

			return( ok );
		}

		public bool GeneraDocumentoNoFiscal(DataTable dtNOFiscal)
		{
			bool ok = false;

			switch (codfiscal)
			{
				case "BEMATECH":
					ok = bemafi32.GeneraDocumentoNoFiscal(dtNOFiscal);
					break;
				case "HASAR":
					ok = hasar.GeneraDocumentoNoFiscal(puerto,dtNOFiscal);
					break;
				case "PNP":
					//ok = pnp.GeneraDocumentoNoFiscal(dtNOFiscal);
					break;
				case "TFHKA": case "BIXOLON350": case "DASCOM230":
					ok = tfhka.GeneraDocumentoNoFiscal(puerto,dtNOFiscal);
					break;
			}

			return( ok );
		}

		public string NumSerial(bool leerMemoria)
		{
			string serial = "";

			switch (codfiscal)
			{
				case "BEMATECH":
					serial = bemafi32.NumSerial();
					break;
				case "HASAR":
					serial = hasar.NumSerial(puerto);
					break;
				case "PNP":
					serial = pnp.NumSerial(puerto);
					break;
				case "TFHKA": case "BIXOLON350": case "DASCOM230":
					serial = tfhka.NumSerial(codfiscal,puerto,leerMemoria);
					break;
			}

			return( serial );
		}

		public string UltNumFactura(bool leerMemoria)
		{
			string numero = "";

			switch (codfiscal)
			{
				case "BEMATECH":
					numero = bemafi32.UltNumFactura();
					break;
				case "HASAR":
					numero = hasar.LeerMemoria(puerto,leerMemoria,7);
					break;
				case "PNP":
					numero = pnp.UltNumFactura(puerto);
					break;
				case "TFHKA": case "BIXOLON350": case "DASCOM230":
					numero = tfhka.UltNumFactura(codfiscal,puerto,leerMemoria);
					break;
			}

			return( numero );
		}

		public string UltNumNotaCredito(bool leerMemoria)
		{
			string numero = "";
			int    d, h;

			switch (codfiscal)
			{
				case "BEMATECH":
					numero = bemafi32.UltNumNotaCredito();
					break;
				case "HASAR":
					numero = hasar.LeerMemoria(puerto,leerMemoria,9);
					break;
				case "PNP":
					numero = pnp.UltNumNotaCredito(puerto);
					break;
				case "TFHKA": case "BIXOLON350": case "DASCOM230":
					numero = tfhka.UltNumNotaCredito(codfiscal,puerto,leerMemoria);
					break;
			}

			return( numero );
		}

		public string UltNumRepZ(bool leerMemoria)
		{
			string numero = "";
			int    d, h;

			switch (codfiscal)
			{
				case "BEMATECH":
					numero = bemafi32.UltNumRepZ();
					break;
				case "HASAR":
					numero = hasar.LeerMemoria(puerto,leerMemoria,6);
					break;
				case "PNP":
					numero = "";
					break;
				case "TFHKA": case "BIXOLON350": case "DASCOM230":
					numero = tfhka.UltNumRepZ(codfiscal,puerto,leerMemoria);
					break;
			}

			return( numero );
		}

		public string FechaFiscal(bool leerMemoria)
		{
			string fecha = "";

			switch (codfiscal)
			{
				case "BEMATECH":
					fecha = bemafi32.FechaFiscal();
					break;
				case "HASAR":
					fecha = hasar.FechaFiscal(puerto);
					break;
				case "PNP":
					fecha = "";
					break;
				case "TFHKA": case "BIXOLON350": case "DASCOM230":
					fecha = tfhka.FechaFiscal(codfiscal,puerto,leerMemoria);
					break;
			}

			return( fecha );
		}

		public bool ReporteX()
		{
			bool ok = false;

			switch (codfiscal)
			{
				case "BEMATECH":
					ok = bemafi32.ReporteX();
					break;
				case "HASAR":
					ok = hasar.ReporteX(puerto);
					break;
				case "PNP":
					ok = pnp.ReporteX(puerto);
					break;
				case "TFHKA": case "BIXOLON350": case "DASCOM230":
					ok = tfhka.ReporteX(puerto);
					break;
			}

			return( ok );
		}

		public bool ReporteZ()
		{
			bool ok = false;

			if (File.Exists("inforepz.xml")) File.Delete("inforepz.xml");

			switch (codfiscal)
			{
				case "BEMATECH":
					ok = bemafi32.ReporteZ();
					break;
				case "HASAR":
					ok = hasar.ReporteZ(puerto);
					break;
				case "PNP":
					ok = pnp.ExtraerRepZ(puerto);
					ok = pnp.ReporteZ(puerto);
					break;
				case "TFHKA": case "BIXOLON350": case "DASCOM230":
					//ok = tfhka.ExtraerRepZ(puerto);
					ok = tfhka.ReporteZ(puerto);
					break;
			}

			return( ok );
		}

		public bool abrir_gaveta()
		{
			bool ok = false;

			switch (codfiscal)
			{
				case "BEMATECH":
					ok = bemafi32.abrir_gaveta();
					break;
				case "HASAR":
					ok = hasar.abrir_gaveta(puerto);
					break;
				case "PNP":
					ok = pnp.abrir_gaveta(puerto);
					break;
				case "TFHKA": case "BIXOLON350": case "DASCOM230":
					ok = tfhka.abrir_gaveta(puerto);
					break;
			}

			return( ok );
		}

		public bool CancelaDoc()
		{
			bool ok = false;

			switch (codfiscal)
			{
				case "BEMATECH":
					ok = bemafi32.CancelaDoc();
					break;
				case "HASAR":
					ok = hasar.CancelaDoc();
					break;
				case "PNP":
					ok = pnp.CancelaDoc(puerto);
					break;
			}

			return( ok );
		}

		public bool Reimprimir(int tipodoc,bool porfecha,string desde,string hasta)
		{
			bool ok = false;

			switch (codfiscal)
			{
				case "TFHKA": case "BIXOLON350": case "DASCOM230":
					ok = tfhka.Reimprimir(puerto,tipodoc,porfecha,desde,hasta);
					break;
			}

			return( ok );
		}

		public static bool RepZtoXML(string repznum,decimal exento,
		                             decimal base1,decimal iva1,
		                             decimal base2,decimal iva2,
		                             decimal base3,decimal iva3,
		                             decimal ncexento,
		                             decimal ncbase1,decimal nciva1,
		                             decimal ncbase2,decimal nciva2,
		                             decimal ncbase3,decimal nciva3,
		                             string ultfactnum,DateTime ultfactfecha,
		                             string ultncnum,DateTime ultncfecha)
		{
			bool ok = true;

			XmlTextWriter myXml = new XmlTextWriter("inforepz.xml",System.Text.Encoding.UTF8);

			myXml.Formatting  = Formatting.Indented;
			myXml.WriteStartDocument(false);
       		myXml.WriteComment("reporte z");

			myXml.WriteStartElement("reportez");

			myXml.WriteElementString("emision",DateTime.Now.ToString());
			myXml.WriteElementString("repznum",repznum);
			myXml.WriteElementString("exento",exento.ToString("#####0.00"));
			myXml.WriteElementString("base1",base1.ToString("#####0.00"));
			myXml.WriteElementString("iva1",iva1.ToString("#####0.00"));
			myXml.WriteElementString("base2",base2.ToString("#####0.00"));
			myXml.WriteElementString("iva2",iva2.ToString("#####0.00"));
			myXml.WriteElementString("base3",base3.ToString("#####0.00"));
			myXml.WriteElementString("iva3",iva3.ToString("#####0.00"));
			myXml.WriteElementString("ncexento",ncexento.ToString("#####0.00"));
			myXml.WriteElementString("ncbase1",ncbase1.ToString("#####0.00"));
			myXml.WriteElementString("nciva1",nciva1.ToString("#####0.00"));
			myXml.WriteElementString("ncbase2",ncbase2.ToString("#####0.00"));
			myXml.WriteElementString("nciva2",nciva2.ToString("#####0.00"));
			myXml.WriteElementString("ncbase3",ncbase3.ToString("#####0.00"));
			myXml.WriteElementString("nciva3",nciva3.ToString("#####0.00"));
			myXml.WriteElementString("ultfactnum",ultfactnum);
			myXml.WriteElementString("ultfactfecha",ultfactfecha.ToString());
			myXml.WriteElementString("ultncnum",ultncnum);
			myXml.WriteElementString("ultncfecha",ultncfecha.ToString());

			myXml.WriteEndElement();

			myXml.WriteEndDocument();
			myXml.Flush();
			myXml.Close();

			return( ok );
		}

	}
}