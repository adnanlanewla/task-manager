const { defineConfig } = require('@vue/cli-service')
module.exports = defineConfig({
  transpileDependencies: true,
  devServer: {
    proxy: {
      '/api': {
        target: 'https://localhost:7049',
        secure: false, // Required for self-signed certificates
        changeOrigin: true,
        pathRewrite: { '^/api': '' },
      },
    },
  },
})
