const path = require("path");
const HtmlWebpackPlugin = require('html-webpack-plugin');

module.exports = {
  mode: "development",
  entry: "./app.fsproj",
  output: {
    path: path.join(__dirname, "./public"),
    filename: "bundle.js",
  },
  devServer: {
    contentBase: "./public",
    port: 8080,
  },
  module: {
    rules: [
      {
        test: /\.fs(x|proj)?$/,
        use: "fable-loader"
      }
    ]
  },
  plugins: [ new HtmlWebpackPlugin({ template: 'index.ejs' }) ]
}
