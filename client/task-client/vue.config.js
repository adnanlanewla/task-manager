const { defineConfig } = require('@vue/cli-service')
module.exports = defineConfig({
  transpileDependencies: true,
  devServer: {
    proxy: {
      '/api': {
        target: 'http://localhost:5144',
        secure: false, // Required for self-signed certificates
        changeOrigin: true,
        pathRewrite: { '^/api': '' },
      },
    },
  },
})
