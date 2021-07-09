using Microsoft.VisualStudio.TestTools.UnitTesting;
using Miris.SwiftMtReader.Model;
using Miris.SwiftMtReader.Reader;
using System.Linq;

namespace Miris.SwiftMtReader.Tests
{
    [TestClass]
    public class SwiftMtMessageOldApiExtensionsTests
    {

        [TestMethod]
        public void GetFileBlock_OK()
        {
            var reader = new SwiftMtMessageReader().Read(SampleMTMessage);

            Assert.AreEqual("{1:F01MFEXSES0XXXX0000000000}", reader.GetFileBlock(1));
            Assert.AreEqual("{2:I515XXXXXXXXXXXXN}", reader.GetFileBlock(2));
        }


        /*
         * ======================
         *  NOT RETRO COMPATIBLE
         * ======================
         *
        [TestMethod]
        public void GetTextBlock_OK()
        {
            var reader = new SwiftMtMessageReader();

            var expectedLink1 = @":16R:LINK
  :20C::RELA//6F201026UAADO
  :16S:LINK";

            var expectedLink2 = @":16R:LINK
  :20C::TRRF//58539998
  :16S:LINK";

            var expectedGenl = $@":16R:GENL
  :20C::SEME//58539998-9996719
  :23G:CANC
  :98C::PREP//20210215122903
  :22F::TRTR//TRAD
  {expectedLink1}
  {expectedLink2}
  :16S:GENL";


            // #1
            var mtMsg = reader.Read(SampleMTMessage);
            var blocks = mtMsg.GetTextBlocks("GENL");
            Assert.AreEqual(1, blocks.Count);
            var genlBlock = blocks.Single();
            Assert.AreEqual(expectedGenl, genlBlock);

            // #2
            blocks = reader.GetTextBlocks(genlBlock, "LINK");
            Assert.AreEqual(2, blocks.Count);
            Assert.AreEqual(expectedLink1, blocks[0]);
            Assert.AreEqual(expectedLink2, blocks[1]);

            // #3 
            Assert.AreEqual(2, reader.GetTextBlocks(SampleMTMessage, @"GENL\LINK").Count);
            Assert.AreEqual(expectedLink1, reader.GetTextBlocks(SampleMTMessage, @"GENL\LINK")[0]);
            Assert.AreEqual(expectedLink2, reader.GetTextBlocks(SampleMTMessage, @"GENL\LINK")[1]);

        }
        // */


        [TestMethod]
        public void GetFields_OK()
        {
            GetFields_OK(SampleMTMessage);
            //GetFields_OK(SampleMTMessageWithNoLineBreaks);                              // NOT RETRO COMPATIBLE
        }

        public void GetFields_OK(string mtMessage)
        {
            var reader = new SwiftMtMessageReader().Read(mtMessage);

            // with qualifier
            reader.GetFields("20C", "RELA");
            Assert.AreEqual(1, reader.GetFields("20C", "RELA").Count);
            Assert.AreEqual("6F201026UAADO", reader.GetFields("20C", "RELA").Single());

            // without qualifier
            Assert.AreEqual(1, reader.GetFields("23G").Count);
            Assert.AreEqual("CANC", reader.GetFields("23G").Single());

            Assert.AreEqual(3, reader.GetFields("20C").Count);
            //Assert.AreEqual(":SEME//58539998-9996719", mtMsg.GetFields("20C")[0]);      // NOT RETRO COMPATIBLE
            //Assert.AreEqual(":RELA//6F201026UAADO", mtMsg.GetFields("20C")[1]);         // NOT RETRO COMPATIBLE
            //Assert.AreEqual(":TRRF//58539998", mtMsg.GetFields("20C")[2]);              // NOT RETRO COMPATIBLE

            // within block
            Assert.AreEqual(5, reader.GetFields("95P").Count);
            Assert.AreEqual(1, reader.GetFields("95P", null, "CONFDET\\CONFPRTY").Count);
            Assert.AreEqual(1, reader.GetFields("95P", "SELL", "CONFDET\\CONFPRTY").Count);
            Assert.AreEqual("MFEXSES0", reader.GetFields("95P", "SELL", "CONFDET\\CONFPRTY").Single());

            Assert.AreEqual(2, reader.GetFields("95P", null, @"SETDET\SETPRTY").Count);
            //Assert.AreEqual(":BUYR//", mtMsg.GetFields("95P", null, @"SETDET\SETPRTY")[0]);  // NOT RETRO COMPATIBLE
            //Assert.AreEqual(":REAG//", mtMsg.GetFields("95P", null, @"SETDET\SETPRTY")[1]);  // NOT RETRO COMPATIBLE

            Assert.AreEqual(1, reader.GetFields("95P", "REAG", @"SETDET\SETPRTY").Count);
            Assert.AreEqual("", reader.GetFields("95P", "REAG", @"SETDET\SETPRTY")[0]);

            // multiline field
            Assert.AreEqual(
                "ISIN LU1597245148  ALLIANZ GIFS-VOLATILITY STR -PT2-  CAP",
                reader.GetField("35B", blockPath: "CONFDET").Replace("\r\n", ""));
        }

        [TestMethod]
        public void GetMessageType_OK()
        {
            var reader = new SwiftMtMessageReader().Read(SampleMTMessage);
            Assert.AreEqual("515", reader.GetMessageType());
        }


        public static string SampleMTMessage => @"{1:F01MFEXSES0XXXX0000000000}{2:I515XXXXXXXXXXXXN}{4:
:16R:GENL
:20C::SEME//58539998-9996719
:23G:CANC
:98C::PREP//20210215122903
:22F::TRTR//TRAD
:16R:LINK
:20C::RELA//6F201026UAADO
:16S:LINK
:16R:LINK
:20C::TRRF//58539998
:16S:LINK
:16S:GENL
:16R:CONFDET
:98A::TRAD//20201027
:98A::NAVD//20201027
:98A::SETT//20201029
:90B::DEAL//ACTU/EUR922,88
:22H::BUSE//SUBS
:22H::PAYM//APMT
:22F::PRIC/SMPG/NAVP
:11A::FXIB//EUR
:16R:CONFPRTY
:95P::SELL//MFEXSES0
:97A::SAFE//70844629DEPST00
:16S:CONFPRTY
:36B::CONF//UNIT/80,083
:35B:ISIN LU1597245148  ALLIANZ GIFS-VOLATILITY STR -PT2-  
CAP
:16R:FIA
:11A::DENO//EUR
:16S:FIA
:16S:CONFDET
:16R:SETDET
:22F::SETR//TRAD
:16R:SETPRTY
:95P::BUYR//
:16S:SETPRTY
:16R:SETPRTY
:95P::REAG//
:16S:SETPRTY
:16R:CSHPRTY
:95P::PAYE//
:16S:CSHPRTY
:16R:CSHPRTY
:95P::BENM//MFEXSES0
:16S:CSHPRTY
:16R:AMT
:19A::DEAL//EUR73907,
:16S:AMT
:16R:AMT
:19A::SETT//EUR73907,
:98A::VALU//20201029
:16S:AMT
:16S:SETDET
-}";

        public string SampleMTMessageWithNoLineBreaks => @"{1:F01MFEXSES0XXXX0000000000}{2:I515XXXXXXXXXXXXN}{4::16R:GENL:20C::SEME//58539998-9996719:23G:CANC:98C::PREP//20210215122903:22F::TRTR//TRAD:16R:LINK:20C::RELA//6F201026UAADO:16S:LINK:16R:LINK:20C::TRRF//58539998:16S:LINK:16S:GENL:16R:CONFDET:98A::TRAD//20201027:98A::NAVD//20201027:98A::SETT//20201029:90B::DEAL//ACTU/EUR922,88:22H::BUSE//SUBS:22H::PAYM//APMT:22F::PRIC/SMPG/NAVP:11A::FXIB//EUR:16R:CONFPRTY:95P::SELL//MFEXSES0:97A::SAFE//70844629DEPST00:16S:CONFPRTY:36B::CONF//UNIT/80,083:35B:ISIN LU1597245148  ALLIANZ GIFS-VOLATILITY STR -PT2-  CAP:16R:FIA:11A::DENO//EUR:16S:FIA:16S:CONFDET:16R:SETDET:22F::SETR//TRAD:16R:SETPRTY:95P::BUYR//:16S:SETPRTY:16R:SETPRTY:95P::REAG//:16S:SETPRTY:16R:CSHPRTY:95P::PAYE//:16S:CSHPRTY:16R:CSHPRTY:95P::BENM//MFEXSES0:16S:CSHPRTY:16R:AMT:19A::DEAL//EUR73907,:16S:AMT:16R:AMT:19A::SETT//EUR73907,:98A::VALU//20201029:16S:AMT:16S:SETDET  -}";

    }
}
