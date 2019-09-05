# C# SDK for Libra blockchain

This is the Libra C# library which connects to the validator-node through the gRPC.

## How to use it

1. Clone repository
```bash
git clone https://github.com/Rio-dapp/libra-csharp-sdk.git
```
2. Open **Example.sln** in Visual Studio 2017 or another IDE. This project use [**.Net Core 2.2**](https://dotnet.microsoft.com/download/dotnet-core/2.2)
3. Select **Examples.CLI** and press **F5**.

## Possibilities

1. LCS example

## Example

```csharp
///---------------------
            /// LCS example with program
            ///---------------------
            byte[] trxWithProgram = "200000003A24A61E05D129CACE9E0EFC8BC9E33831FEC9A9BE66F50FD352A2638A49B9EE200000000000000000000000040000006D6F766502000000020000000900000043414645204430304402000000090000006361666520643030640300000001000000CA02000000FED0010000000D1027000000000000204E0000000000008051010000000000"
                 .ToLower().HexStringToByteArray();
            int corsor = 0;
            RawTransactionLCS rawTransactionLCS
                = trxWithProgram.LCSerialization<RawTransactionLCS>(ref corsor);
            Console.WriteLine("with program = " + rawTransactionLCS);
```

## This Sdk is used in the followwing example:
[libraview.org](https://libraview.org)

