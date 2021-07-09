using Microsoft.VisualStudio.TestTools.UnitTesting;
using Miris.SwiftMtReader.Reader;
using System.IO;

namespace Miris.SwiftMtReader.Tests
{
    [TestClass]
    public class SwiftMtMessageReaderTests
    {
        [TestMethod]
        public void SwiftMtMessageReader_OK()
        {
            var candidate = new SwiftMtMessageReader();

            var mtMsg = candidate.Read(new StringReader(SampleMTMessage));
            Assert.IsNotNull(mtMsg);
            Assert.AreEqual(4, mtMsg.Blocks.Count);
        }

        public static string SampleMTMessage => @"{1:F01AXLTFRPPAXXX0740191043}{2:O5350316210615MGTCBEBEBFND68552932022106150316N}{4:
:16R:GENL
    :28E:4/MORE
    :20C::SEME//FUNDS23027T01674
    :23G:NEWM
    :98C::STAT//20210614235959
    :22F::SFRE//DAIL
    :22F::CODE//COMP
    :22F::STTY//ACCTa
    :22F::STBA//SETT
    :97A::SAFE//23027
    :17B::ACTI//Y
    :17B::CONS//Y
:16S:GENL
:16R:SUBSAFE
    :97A::SAFE//None
    :17B::ACTI//Y
    :16R:FIN
        :35B:ISIN IE00B067MS69
        PRINCIPAL GIF-PREFERRED SEC.(A)INC
        :22H::CAOP//CASH
        :90B::MRKT//ACTU/USD9,39
        :98A::PRIC//20210611
        :93B::AGGR//UNIT/3161,
        :16R:SUBBAL
            :93C::NOMI//UNIT/AVAI/3161,
            :90B::MRKT//ACTU/USD9,39
            :19A::HOLD//USD29681,79
            :70C::SUBB///DECL/000787-0262
            /STBR/None
        :16S:SUBBAL
        :19A::HOLD//USD29681,79
    :16S:FIN
    :16R:FIN
        :35B:ISIN IE00B06YCB08
        BNY MELLON GL-EMERGING MKTS DBT(C)
        :22H::CAOP//DRIP
        :90B::MRKT//ACTU/USD2,6958
        :98A::PRIC//20210611
        :93B::AGGR//UNIT/294828,705
        :16R:SUBBAL
            :93C::NOMI//UNIT/AVAI/294828,705
            :90B::MRKT//ACTU/USD2,6958
            :19A::HOLD//USD794799,22
            :70C::SUBB///DECL/05000004-0866
            /STBR/None
        :16S:SUBBAL
        :19A::HOLD//USD794799,22
    :16S:FIN
:16S:SUBSAFE
-}{5:{CHK:08062CEE30AC}}";
    }
}
