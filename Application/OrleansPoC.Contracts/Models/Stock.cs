namespace OrleansPoC.Contracts.Models;

[GenerateSerializer, Alias("OrleansPoC.Contracts.Models.Stock")]
public class Stock
{
    [Id(0)]
    public string Name { get; set; } = string.Empty;
    [Id(1)]
    public decimal Value { get; set; }
    
    public static string[] GetStockList() => 
    [
        "ABEV3", "AZUL4", "B3SA3", "BBAS3", "BBDC3", "BBDC4", "BBSE3", "BPAC11",
        "BRAP4", "BRDT3", "BRFS3", "BRKM5", "CAML3", "CCRO3", "CIEL3", "CMIG4",
        "COGN3", "CPFE3", "CRFB3", "CSAN3", "CSNA3", "CYRE3", "ECOR3", "EGIE3",
        "ELET3", "ELET6", "EMBR3", "ENBR3", "EQTL3", "FLRY3", "GGBR4", "GOAU4",
        "GOLL4", "HAPV3", "HGTX3", "HYPE3", "IRBR3", "ITSA4", "ITUB4", "JBSS3",
        "KLBN11", "LAME4", "LREN3", "MGLU3", "MULT3", "NTCO3", "PCAR3", "PETR3",
        "PETR4", "QUAL3", "RADL3", "RAIL3", "RENT3", "SANB11", "SBSP3", "SULA11",
    ];
}

/*
        "SUZB3", "TAEE11", "TIMP3", "TOTS3", "UGPA3", "USIM5", "VALE3", "VIVT3",
   "VVAR3", "WEGE3", "YDUQ3", "PETZ3", "RAIL3", "TEND3", "VIVA3", "LOGG3",
   "IRBR3", "HAPV3", "BIDI11", "BPAN4", "BRSR6", "CRIV3", "CSMG3", "DTEX3",
   "ENJU3", "EUCA4", "FESA4", "FRAS3", "GRND3", "GUAR3", "HBOR3", "JHSF3",
   "JSLG3", "LCAM3", "LEVE3", "LOGN3", "MYPK3", "ODPV3", "OMGE3", "PARD3",
   "PATI3", "POMO4", "POSI3", "PRNR3",
*/