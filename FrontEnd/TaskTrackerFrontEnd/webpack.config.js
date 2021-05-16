const webpack = require('webpack');

module.exports = {
  plugins: [
    new webpack.DefinePlugin({
      'STABLE_FEATURE': JSON.stringify(true),
      'EXPERIMENTAL_FEATURE': JSON.stringify(false)
    })
  ],
  module: {
    rules: [
      {
        test: /\.css$/,
        loader: "postcss-loader",
        options: {
          postcssOptions: {
            ident: "postcss",
            plugins: () => [require("tailwindcss"), require("autoprefixer")],
          },
        },
      },
    ],
  },
}