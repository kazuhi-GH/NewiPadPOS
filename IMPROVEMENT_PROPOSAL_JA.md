# 改善提案（ソースコード解析ベース）

解析日: 2026-06-04

## 優先度A（先に対応）

1. **在庫更新処理を原子的にする**
   - 対象: `Services/OrderService.cs`（`CreateOrderAsync`）
   - 現状: 在庫確認と減算が注文ループ内で個別に実行され、同時注文時に競合の可能性があります。
   - 提案: トランザクション + 楽観的同時実行制御（または行ロック）で在庫更新を一括処理。

2. **注文番号採番の競合対策**
   - 対象: `Services/OrderService.cs`（`GenerateOrderNumberAsync`）
   - 現状: 当日件数カウント + 1 で採番しているため、同時実行で重複の可能性があります。
   - 提案: DBシーケンス/連番テーブル方式へ変更し、ユニーク制約違反時の再試行も実装。

3. **時刻取得の統一（UTC/Clock注入）**
   - 対象: `Services/OrderService.cs`, `Services/ProductService.cs`, `Models/Order.cs`, `Models/Product.cs`
   - 現状: `DateTime.Now` / `DateTime.Today` に依存しています。
   - 提案: `TimeProvider` などをDIし、UTC基準で保存・変換表示に統一（テスト性も向上）。

## 優先度B（早めに対応）

4. **レシートHTML生成の重複を統合**
   - 対象: `Pages/Index.razor`（`GenerateHtmlReceipt` と `GenerateReceiptHtml`）
   - 現状: 同種のHTML生成ロジックが2系統あり、店舗情報も不一致です。
   - 提案: 1メソッドへ統合し、店舗情報は設定ファイル化。

5. **ハードコード値の設定外出し**
   - 対象: `Pages/Index.razor`, `Models/Cart.cs`, `Services/EmailService.cs`
   - 現状: 店舗名/住所/電話、税率(10%)、メール件名などが固定値です。
   - 提案: `appsettings.json` + Optionsパターンへ移行。

6. **メール送信入力のバリデーション強化**
   - 対象: `Services/EmailService.cs`
   - 現状: 宛先メール形式の検証がありません。
   - 提案: 形式チェックと失敗時メッセージ標準化（ログは構造化ログへ）。

## 優先度C（継続改善）

7. **決済シミュレーション乱数の改善**
   - 対象: `Pages/Index.razor`（`new Random().Next(100)`）
   - 提案: `Random.Shared` を利用し、偏りと再現性問題を軽減。

8. **テスト基盤の追加（サービス層から）**
   - 対象: `OrderService`, `Cart`, `ProductService`
   - 提案: 最低限、在庫更新・採番・税計算の単体テストを追加。

---

## 推奨実施順

1. 在庫更新の原子性
2. 採番競合対策
3. 時刻取得統一
4. レシート統合 + 設定外出し
5. バリデーション/乱数改善
6. テスト追加
