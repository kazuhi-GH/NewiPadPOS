# .NET Framework 4.8 MVC サンプル

このディレクトリには、C# で作成した **ASP.NET MVC 5 / .NET Framework 4.8** の最小サンプルがあります。

## 構成

- `Net48MvcSample.csproj` : .NET Framework 4.8 向けプロジェクト
- `Global.asax` / `RouteConfig.cs` : アプリ起動・ルーティング
- `Controllers/HomeController.cs` : サンプルコントローラー
- `Models/MessageViewModel.cs` : サンプルViewModel
- `Views/Home/Index.cshtml` : サンプル画面

## 実行方法（Windows + Visual Studio 2022 推奨）

1. Visual Studio で `samples/Net48MvcSample/Net48MvcSample.csproj` を開く
2. NuGet パッケージを復元する
3. IIS Express で実行する
4. `https://localhost:<port>/` にアクセスする

## 補足

- 本サンプルは .NET Framework 4.8 向けのため、Linux 環境では実行できません。
- 学習用の最小構成です。実運用では認証、ロギング、例外処理、テストを追加してください。
