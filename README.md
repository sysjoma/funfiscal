# funfiscal
driver para manejo de impresoras fiscales hasar, bematech, pnp, y modelos the factory.  Genera lo siguiente: Facturas, Notas de crédito, Reporte X, Reporte Z y documentos no fiscales.  Permite capturar los datos del reporte Z.

Ha sido probado en impresoras fiscales de Venezuela y Panamá.

# Especificaciones técnicas
- IDE de desarrollo SharpDevelop 5.1 RC.
- .NET Framework 4.0.

# Licencia
Licencia open source.

# Contacto
Desarrollado por José Alemán
sysjoma@gmail.com


Ejemplo de como generar una factura fiscal

	private bool ImprimirFacturaFiscal()
	{
	bool   ok = false;
	string docufiscal, serialfiscal, numerorepz;

	docufiscal = serialfiscal = nurepz = "";

	DataTable dtDocFiscal = new DataTable();

	dtDocFiscal.Columns.Add("codigo",typeof(string));
	dtDocFiscal.Columns.Add("descrip",typeof(string));
	dtDocFiscal.Columns.Add("precio",typeof(decimal));
	dtDocFiscal.Columns.Add("cant",typeof(decimal));
	dtDocFiscal.Columns.Add("tasaiva",typeof(decimal));
	dtDocFiscal.Columns.Add("baseiva",typeof(int));
	dtDocFiscal.Columns.Add("talla",typeof(string));
	dtDocFiscal.Columns.Add("color",typeof(string));			
	dtDocFiscal.Columns.Add("seriales",typeof(string));
	dtDocFiscal.Columns.Add("notas",typeof(string));

	dtDocfiscal.Rows.Add(new object[] {"001","LAPTOP LENOVO 3000",500,1,12,1,"","","GARANTIA 6 MESES"});

	fiscal fis     = new fiscal("BIXOLON30","1");

	fis.referencia = misvariables.referencia;
	fis.rifoci     = "V17234670";
	fis.nomcli     = "MANUEL PEREZ";
	fis.dircli     = "PUERTO LA CRUZ";
	fis.tlfcli     = "34561200";
	fis.vendedor   = "ANAMARIA";
	fis.condipago  = "CONTADO";
	fis.totaldoc   = Convert.ToDecimal(texTotalFactura.Text);
	fis.fechavence = emision.AddDays(diasvence);
	fis.dtDetalle  = dtDocFiscal;
	fis.dtPagos    = null;
	fis.cajero     = misvariables.user;
	fis.estacion   = misvariables.station;
	fis.mensajePie = misvariables.mensajeTicketFiscal;
	ok             = fis.GeneraFactura();

	if ( ok )
	{
		serialfiscal = fis.NumSerial(true).Trim();
		docufiscal   = fis.UltNumFactura(false).Trim();
		numerorepz   = fis.UltNumRepZ(false).Trim();
	}
	else
	{
		DialogResult SiNo;

		SiNo = MessageBox.Show("¿ La factura fiscal fue emitida ?","Conforme",
	                       	   MessageBoxButtons.YesNo,MessageBoxIcon.Question,
	                       	   MessageBoxDefaultButton.Button2);

		ok   = (SiNo == DialogResult.Yes);
	}

	return ok;
	}


Ejemplo de como emitir un Reporte X

	void BtnReporteXClick(object sender, EventArgs e)
	{
		bool ok = false;

		ok = fis.ReporteX();

		if (!ok)
		{
			// ...mensaje...
		}
	}


