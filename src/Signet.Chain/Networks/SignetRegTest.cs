//
//  Signet.FullNode - Blockchain / Cryptocurrency Platform
//  Portions Copyright © 2019 Synuit Software. All Rights Reserved.
//
//  $!!$ tac
//
using System;
using System.Collections.Generic;
using NBitcoin;
using NBitcoin.BouncyCastle.Math;
using NBitcoin.Protocol;

namespace Signet.Networks
{
   public class SignetRegTest : SignetMain
   {
      public SignetRegTest()
      {
            NetworkConfiguration config = new NetworkConfigurations().GetNetwork("regtest", "signet");
            this.Name = "SignetRegTest";
         this.NetworkType = NetworkType.Regtest;
         this.Magic = 0x2e545347; // .TSG
         this.DefaultPort = config.Port;
         this.DefaultRPCPort = config.RpcPort;
         this.DefaultAPIPort = config.ApiPort;
         this.DefaultSignalRPort = config.WsPort;
         this.MinTxFee = 0;
         this.FallbackFee = 0;
         this.MinRelayTxFee = 0;
         this.CoinTicker = "TSGT";
         this.DefaultBanTimeSeconds = 16000; // 500 (MaxReorg) * 64 (TargetSpacing) / 2 = 4 hours, 26 minutes and 40 seconds

         var powLimit = new Target(new uint256("7fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff"));

         var consensusFactory = new PosConsensusFactory();

         // Create the genesis block.
         this.GenesisTime = 1570492222; 
         this.GenesisNonce = 17287;
         this.GenesisBits = 0x1F00FFFF;
         this.GenesisVersion = 1;
         this.GenesisReward = Money.Zero;

        
         // 2019-09-24: "GE Healthcare officially launches Edison AI platform in China"
         // URL: https://www.healthcareitnews.com/news/asia-pacific/ge-healthcare-officially-launches-edison-ai-platform-china
         var pszTimestamp = "Sept. 24, 2019, Healthcare IT News, GE Healthcare officially launches Edison AI ...";
         
     
         Block genesisBlock = CreateSignetGenesisBlock(pszTimestamp, consensusFactory, this.GenesisTime, this.GenesisNonce, this.GenesisBits, this.GenesisVersion, this.GenesisReward);

         this.Genesis = genesisBlock;

         var consensusOptions = new SignetPosConsensusOptions(
             maxBlockBaseSize: 1_000_000,
             maxStandardVersion: 2,
             maxStandardTxWeight: 100_000,
             maxBlockSigopsCost: 20_000,
             maxStandardTxSigopsCost: 20_000 / 5
         );

         var buriedDeployments = new BuriedDeploymentsArray
         {
            [BuriedDeployments.BIP34] = 0,
            [BuriedDeployments.BIP65] = 0,
            [BuriedDeployments.BIP66] = 0
         };

         var bip9Deployments = new SignetBIP9Deployments()
         {
            [SignetBIP9Deployments.ColdStaking] = new BIP9DeploymentsParameters("ColdStaking", 2,
                 new DateTime(2018, 12, 1, 0, 0, 0, DateTimeKind.Utc),
                 new DateTime(2019, 12, 1, 0, 0, 0, DateTimeKind.Utc))
         };

         this.Consensus = new Consensus(
            consensusFactory: consensusFactory,
            consensusOptions: consensusOptions,
            coinType: 1973, //$!!$ tac
            hashGenesisBlock: genesisBlock.GetHash(),
            subsidyHalvingInterval: 210000,
            majorityEnforceBlockUpgrade: 750,
            majorityRejectBlockOutdated: 950,
            majorityWindow: 1000,
            buriedDeployments: buriedDeployments,
            bip9Deployments: bip9Deployments,
            bip34Hash: new uint256("0x0000000000000000000000000000000000000000000000000000000000000000"), // $!!$ tac
            ruleChangeActivationThreshold: 1916, // 95% of 2016
            minerConfirmationWindow: 2016, // nPowTargetTimespan / nPowTargetSpacing
            maxReorgLength: 500,
            defaultAssumeValid: null, // turn off assumevalid for regtest.
            maxMoney: long.MaxValue,
            coinbaseMaturity: 10,
            premineHeight: 2,
            premineReward: Money.Coins(13736000000),
            proofOfWorkReward: Money.Coins(2),
            powTargetTimespan: TimeSpan.FromSeconds(14 * 24 * 60 * 60), // two weeks
            powTargetSpacing: TimeSpan.FromSeconds(10 * 60),
            powAllowMinDifficultyBlocks: true,
            posNoRetargeting: false,
            powNoRetargeting: true,
            powLimit: powLimit,
            minimumChainWork: null,
            isProofOfStake: true,
            lastPowBlock: 125000,
            proofOfStakeLimit: new BigInteger(uint256.Parse("00000fffffffffffffffffffffffffffffffffffffffffffffffffffffffffff").ToBytes(false)),
            proofOfStakeLimitV2: new BigInteger(uint256.Parse("000000000000ffffffffffffffffffffffffffffffffffffffffffffffffffff").ToBytes(false)),
            proofOfStakeReward: Money.Coins(20)
         );

         this.Checkpoints = new Dictionary<int, CheckpointInfo>()
         {
            // Fake checkpoint to prevent PH to be activated.
            // TODO: Once PH is complete, this should be removed
            // { 100_000 , new CheckpointInfo(uint256.Zero, uint256.Zero) }
         };

         this.Base58Prefixes[(int)Base58Type.PUBKEY_ADDRESS] = new byte[] { (65) };  // T
         this.Base58Prefixes[(int)Base58Type.SCRIPT_ADDRESS] = new byte[] { (127) }; // t
         this.Base58Prefixes[(int)Base58Type.SECRET_KEY] = new byte[] { (65 + 128) };// wif ??? $!!$ tac

         this.DNSSeeds = new List<DNSSeedData>
         {
            /* $!!$ tac
            new DNSSeedData("signet-chain.org", "regtestseed.city-chain.org"),
            new DNSSeedData("signet-coin.org", "regtestseed.city-coin.org"),
            new DNSSeedData("signetchain.foundation", "regtestseed.citychain.foundation")
            */
         };

         this.SeedNodes = new List<NetworkAddress>
         {
            /* $!!$ tac
             new NetworkAddress(IPAddress.Parse("13.73.143.193"), this.DefaultPort),
             new NetworkAddress(IPAddress.Parse("40.115.2.6"), this.DefaultPort),
             new NetworkAddress(IPAddress.Parse("13.66.158.6"), this.DefaultPort),
             */
         };

         this.StandardScriptsRegistry = new SignetStandardScriptsRegistry();

         // 64 below should be changed to TargetSpacingSeconds when we move that field.
         Assert(this.DefaultBanTimeSeconds <= this.Consensus.MaxReorgLength * 64 / 2);
         Assert(this.Consensus.HashGenesisBlock == uint256.Parse("0x0000c0b9eaf67a210b46218bad5d3587aab6500d4bc2b91b5b1a48c19ce8f6c7"));
         Assert(this.Genesis.Header.HashMerkleRoot == uint256.Parse("0xa42c30d6615b79c0cc5abd6a9e72e4cf14b29fd4de7d14e62f0ce2bca9f196a8"));

         this.RegisterRules(this.Consensus);
      }
   }
}