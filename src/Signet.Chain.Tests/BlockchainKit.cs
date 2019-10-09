//
//  Signet.FullNode - Blockchain / Cryptocurrency Platform
//  Copyright © 2019 Synuit Software. All Rights Reserved.
//
//  $!!$ tac
//
using System.Collections.Generic;
using NBitcoin;
using Xunit;

namespace Signet.Chain.Tests
{
   public class BlockchainStartup
   {
      // $!!$ tac - genesis blocks have been generated Signet networks, this code does not need to be executed 
      //            again unless an entire reboot of the blockchain is required!
      //$
      //////////////[Fact]
      //////////////public void GenerateGenesisBlocks()
      //////////////{
      //////////////   List<string> contents = new List<string>();

      //////////////   // SIGNET GENESIS BLOCK:
      //////////////   // 2019-09-17: "Elon Musk is the most inspirational leader in tech, new survey shows."
      //////////////   // https://www.cnbc.com/2019/09/17/elon-musk-named-the-most-inspirational-leader-in-tech.html
      //////////////   var urlMain = "Sept. 17, 2019, CNBC.COM, Elon Musk is the most inspirational leader in tech ...";

      //////////////   // SIGNET TEST GENESIS BLOCK:
      //////////////   // 2019-09-24: "'Storm Area 51' has Nevada counties facing potentially hefty bills"
      //////////////   // URL: https://www.foxnews.com/science/storm-area-51-nevada-counties-hefty-bills
      //////////////   var urlTest = "Sept. 24 2019, FoxNews.com, 'Storm Area 51' has Nevada counties facing ...";

      //////////////   // SIGNET REG-TEST GENESIS BLOCK:
      //////////////   // 2019-09-24: "GE Healthcare officially launches Edison AI platform in China"
      //////////////   // URL: https://www.healthcareitnews.com/news/asia-pacific/ge-healthcare-officially-launches-edison-ai-platform-china
      //////////////   var urlRegTest = "Sept. 24, 2019, Healthcare IT News, GE Healthcare officially launches Edison AI ...";

      //////////////   //$!!$ tac uint testTime = 1569362742; // $!!$ tac 09/24/2019 @ 10:05pm (UTC)
      //////////////   var testBlock = Network.MineGenesisBlock(new PosConsensusFactory(), urlTest, new Target(new uint256("000fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff")), Money.Zero); //$!!$tac, testTime);

      //////////////   contents.Add("TEST GENESIS BLOCK:");
      //////////////   contents.Add("Test Nonce: " + testBlock.Header.Nonce);
      //////////////   contents.Add("Test Time: " + testBlock.Header.Time);
      //////////////   contents.Add("Test Bits: " + testBlock.Header.Bits.ToCompact().ToString("X2"));
      //////////////   contents.Add("Test Hash: " + testBlock.Header.ToString());
      //////////////   contents.Add("Test Hash Merkle Root: " + testBlock.Header.HashMerkleRoot);

      //////////////      //$!!$ tac uint regTestTime = 156936278; // $!!$ tac 09/24/2019 @ 10:06pm (UTC)
      //////////////      Block regTestBlock = Network.MineGenesisBlock(new PosConsensusFactory(), urlRegTest, new Target(new uint256("0000ffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff")), Money.Zero); //$!!$tac, regTestTime);

      //////////////   contents.Add("");
      //////////////   contents.Add("TEST REG GENESIS BLOCK:");
      //////////////   contents.Add("Reg Test Nonce: " + regTestBlock.Header.Nonce);
      //////////////   contents.Add("Reg Test Time: " + regTestBlock.Header.Time);
      //////////////   contents.Add("Reg Test Bits: " + regTestBlock.Header.Bits.ToCompact().ToString("X2"));
      //////////////   contents.Add("Reg Test Hash: " + regTestBlock.Header.ToString());
      //////////////   contents.Add("Reg Test Hash Merkle Root: " + regTestBlock.Header.HashMerkleRoot);

      //////////////   //$!!$ tac int mainTime = 1569362824;  // $!!$ tac 09/24/2019 @ 10:07pm (UTC)
      //////////////   var mainBlock = Network.MineGenesisBlock(new PosConsensusFactory(), urlMain, new Target(new uint256("00000fffffffffffffffffffffffffffffffffffffffffffffffffffffffffff")), Money.Zero); //$!!$tac,  mainTime);

      //////////////   contents.Add("");
      //////////////   contents.Add("MAIN GENESIS BLOCK:");
      //////////////   contents.Add("Main Nonce: " + mainBlock.Header.Nonce);
      //////////////   contents.Add("Main Time: " + mainBlock.Header.Time);
      //////////////   contents.Add("Main Bits: " + mainBlock.Header.Bits.ToCompact().ToString("X2"));
      //////////////   contents.Add("Main Hash: " + mainBlock.Header.ToString());
      //////////////   contents.Add("Main Hash Merkle Root: " + mainBlock.Header.HashMerkleRoot);

      //////////////   System.IO.File.AppendAllLines("genesis-output.txt", contents);
      //////////////}
   }
}