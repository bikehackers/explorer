const path = require('path');
const HtmlWebpackPlugin = require('html-webpack-plugin');
const CopyWebpackPlugin = require('copy-webpack-plugin');

const mode = process.env['NODE_ENVIRONMENT'] === 'production' ? 'production' : 'development'

console.log({ mode });

module.exports = {
  mode,
  entry: './app.fsproj',
  output: {
    path: path.join(__dirname, './public'),
    filename: 'bundle.js',
  },
  devServer: {
    contentBase: './public',
    port: 8080,
  },
  module: {
    rules: [
      {
        test: /\.fs(x|proj)?$/,
        use: 'fable-loader'
      }
    ]
  },
  plugins: [
    new HtmlWebpackPlugin({ template: 'index.ejs' }),
    new CopyWebpackPlugin({
      patterns: [
        { from: 'static' },
      ]
    }),
  ],
}
