# AutoEscapeZoom

授業終了時にZoomから自動的に退出します。

## 要件

- Windows 10
- .NET Framework 4.7.2
- 言語パックのOCRのやつ

## 詳細

一定時間(既定値: 10秒)間隔でZoomの参加人数を確認します。Zoomのスクリーンショットを取得し，光学文字認識により参加人数を読み取ります。
ミーティングごとに参加人数の最大数を記録し，参加人数が最大数より一定の割合(既定値: 6割)に減少した場合にミーティングから退出します。

## 使用方法

`AutoEscapeZoom.exe`を起動すると勝手に動きます。
`AutoEscapeZoom.Utils.dll`を[PandAforWin](https://ikuzak.com/ku/panda-for-win/)のプラグインとして使用することも可能です。
