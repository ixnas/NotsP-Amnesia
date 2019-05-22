using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Amnesia.Application.Validation.Context;
using Amnesia.Cryptography;
using Amnesia.Domain.Entity;
using Amnesia.Domain.ViewModels;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Amnesia.Tests.Validation
{
    public class TestData
    {
        public IList<string> Blobs = new List<string>()
        {
            "This is a piece of data",
            "And another!",
            "Such data much info wow"
        };

        private static readonly KeyPair Keys = new KeyPair(
            new PrivateKey(@"﻿-----BEGIN RSA PRIVATE KEY-----
MIIEowIBAAKCAQEA1ADkjzRkMWrzUaR3qYZoCTwLA8z8OdyZkbpMVeWShKQet+28
GgFdYce2GrzlHZLOak3seJCWPfPOtCvNOr7YSdUqB0k3tr5Pc1Li18s/b8VySyKG
ICy5T61bncUudHasxFQKI9QTCpT66HTWTVipVVjA2siB8ohe6+HA+pWqC8LslFAc
Ygp5/nHvUp79xcG29djcpJSQaqrtaJBhjSDMO2rSxtcdP3J9wVMacYiFmL9h1gEX
HvzpFzS+kyZrhRNEXZglGiBv9sDOlDgLTiduuMhmhzAbI0/aKhG4Fbk22sM7ek0o
6cB1gE1VmPLVeI2NuWlwXVL1bHKM7bsRMQZxYQIDAQABAoIBAAQ82FS8eVw7hQ2W
ugWbeCznFFlJGamWbUNqc+grpJgeJ7cJe38Rk+pGca9rIzFKI/IgCKAIU9h9LR6Q
2ZNZCT3Akwx+PCzc9P2bJImnuGbljXlPAVtXSYT8NB6dOGZ/kMiD3YgFzzOg8rr+
yuHU6d2Y3pIA/Af7DdqtMoxPvYxqmbZMXxMvso1hao7vfohKg1x0EbPrKcozVJ+U
8p9hizICRgTD2HlsV3Ac+oEvN9x0CxLdi3bB49Sxy6WPOgh1FbBaa4GPWlSH495s
IblwUyTzyQb6xXwBczfYb8eKM6/mFVdkqskdzzCIDw3+w/QOpw2aTCUwldke0ptX
pY2KIFkCgYEA2bcGkZDmsaj/9nAesc2hILUdCI0mCFScC4hPA2WbvMuKFfrO5ft1
WM2nmOduMTlkhige52/7wzGCpmQDj1Srxb2tltA5fHgzcryipw5LYp81q0tsFh4o
bZyB8O61kjfZuML+w8ueUWXCdWBsdz2vH3uC2M6vgN8wuV2ZpNLFWNsCgYEA+UjA
euEDZNmOH+ZmNufmB2hR4WHps/wLNCo1Njl3RK27/kFPf1KuatLCDdGuQOUKcAVP
jpk+ZRlcCwWJVvl34Z3w1jUFETiMf70BoCHBWzDXhe4HJ0MdafRrgM5MGXpPRwUU
6BdfcWxf9uEDDlaABXJmciMDyjQDvHihcZutxXMCgYBGwW/WESSYapc9TTT8jhqm
mZXk+JJ6tJy+Nr+PGA+kLPYkrI1fOvjpMnUcgWJThxZ/bzyT9NX6mvWeA/UQOPFW
Y42t7OAx7pKx4FJwPnoLhDiaAfoPuh9jeDDWz42dBp+wp1bK5Tr/szWelgbKPfbT
IX3l3k41cNPR+nR7l7BhbwKBgQDTSuwC+0hCKfjCZyqKLWMXMj/81l4ddWwVqhTy
QiEqsEuhBcCZPf6igsOCrRCS5tBDBO1bA98UGDuhB+9SLh+Dt7iUB9qkrxa/ivee
wB3A6pQza/7aM5Px4/9e7P0ptzcNDmybEHyQifiJLE3H0F+wvjfBVJZ4xD/Kd0D4
J6lRqwKBgD9OsBFJc4USGwbyALK1Y1rbtR34gw3Q2IuHSe+KUTA+3SZpuFLu6G2a
+Yxg8hRONhmSSuXn9FcwnBZ+gi7EoU5/zpHlgPcLfJaWZ573s4bG2pbNfcGoyqe8
n1DcNLBIy3L2qzTloFx+HDQEuxXo2Bz3yU4wYNYIBJt6DVLeEmVA
-----END RSA PRIVATE KEY-----"), 
            new PublicKey(@"-----BEGIN PUBLIC KEY-----
MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA1ADkjzRkMWrzUaR3qYZo
CTwLA8z8OdyZkbpMVeWShKQet+28GgFdYce2GrzlHZLOak3seJCWPfPOtCvNOr7Y
SdUqB0k3tr5Pc1Li18s/b8VySyKGICy5T61bncUudHasxFQKI9QTCpT66HTWTVip
VVjA2siB8ohe6+HA+pWqC8LslFAcYgp5/nHvUp79xcG29djcpJSQaqrtaJBhjSDM
O2rSxtcdP3J9wVMacYiFmL9h1gEXHvzpFzS+kyZrhRNEXZglGiBv9sDOlDgLTidu
uMhmhzAbI0/aKhG4Fbk22sM7ek0o6cB1gE1VmPLVeI2NuWlwXVL1bHKM7bsRMQZx
YQIDAQAB
-----END PUBLIC KEY-----"));

        public (MemoryValidationContext, byte[]) ConstructTestData()
        {
            var context = new MemoryValidationContext();

            byte[] prefDef = null;
            var definitions = Blobs.Select(blob =>
            {
                var def = CreateDefinition(blob, Keys, prefDef);
                prefDef = def.Hash;
                return def;
            }).ToList();

            Block prefBlock = null;
            var blocks = definitions.Select(def =>
            {
                var block = CreateBlock(def, prefBlock);
                prefBlock = block;
                return block;
            }).ToList();

            foreach (var block in blocks)
            {
                context.AddBlock(block);
                context.AddContent(block.Content);
            }

            foreach (var definition in definitions)
            {
                context.AddData(definition.Data);
                context.AddDefinition(definition);
            }

            return (context, prefBlock.Hash);
        }

        private static Block CreateBlock(Definition def, Block prefBlock)
        {
            var content = new Content();

            content.Definitions.Add(def.Hash);

            var block = new Block
            {
                Content = content,
                ContentHash = content.Hash,
                Nonce = 0,
                PreviousBlockHash = prefBlock?.Hash,
                PreviousBlock = prefBlock
            };

            return block;
        }

        private static Definition CreateDefinition(string blob, KeyPair keys, byte[] prefDef)
        {
            var data = new Data
            {
                Blob = Encoding.UTF8.GetBytes(blob),
                PreviousDefinitionHash = prefDef,
                Key = keys.PublicKey.ToPEMString()
            };
            data.Signature = keys.PrivateKey.SignData(data.SignatureHash.EncodeToBytes());

            var definition = new Definition
            {
                Data = data,
                DataHash = data.Hash,
                PreviousDefinitionHash = prefDef,
                Key = keys.PublicKey.ToPEMString(),
                Meta = new Dictionary<string, string>
                {
                    ["IsMutable"] = "true",
                    ["IsMutation"] = "false"
                }
            };
            definition.Signature = keys.PrivateKey.SignData(definition.SignatureHash.EncodeToBytes());
            return definition;
        }

        [Test, Explicit]
        public void DumpTestData()
        {
            var (context, startBlock) = ConstructTestData();

            Assert.IsNotNull(context);
            Assert.IsNotNull(startBlock);

            var defViewModels = context.Definitions.Select(pair => new DefinitionViewModel(pair.Value));
            var json = JsonConvert.SerializeObject(defViewModels, Formatting.Indented);

            File.WriteAllText("TestDefinitions.json", json);
        }
    }
}