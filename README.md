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

1. Get Account Info
2. Get Transactions
3. Get Transactions by Seqenc number
4. Send Transaction

## Example

Get Account Info as in example below. GetAccountInfoAsync returns CustomAccountResource object. It is processed data.

```csharp
LibraAdmissionControl service = new LibraAdmissionControl();

var address = "0000000000000000000000000000000000000000000000000000000000000000";
CustomAccountResource account = service.GetAccountInfoAsync(address).Result;
Console.WriteLine(account.Balance);
```
or

```csharp
LibraAdmissionControl service = new LibraAdmissionControl();

var address = "0000000000000000000000000000000000000000000000000000000000000000";
CustomAccountResource account = await service.GetAccountInfoAsync(address);
Console.WriteLine(account.Balance);
```

## If you want to get original Raw data use the LibraAdmissionControlService:

```csharp
LibraAdmissionControlService service = new LibraAdmissionControlService("ac.testnet.libra.org",8000);

var address = "0000000000000000000000000000000000000000000000000000000000000000";
AccountStateWithProof account = await service.GetAccountInfoAsync(address);
Console.WriteLine(account.Proof);
```

## This Sdk is used in the followwing example:
[libraview.org](https://libraview.org)

## TODO
