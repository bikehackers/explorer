const path = require('path');
const HtmlWebpackPlugin = require('html-webpack-plugin');
const CopyWebpackPlugin = require('copy-webpack-plugin');

const mode = process.env['NODE_ENVIRONMENT'] === 'production' ? 'production' : 'development'

console.log({ mode });

module.exports = {
  mode,
  entry: './App.fs.js',
  output: {
    path: path.join(__dirname, './public'),
    filename: 'bundle.js',
  },
  devServer: {
    contentBase: './public',
    port: 8080,
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
