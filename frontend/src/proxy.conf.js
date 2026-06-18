/*{
    "/api/**": {
        "target": "https://instafake-bff.dev.localhost:7153",
        "secure": false,
        "pathRewrite": {
            "^/api": ""
        }
    }
}*/

const PROXY_CONFIG = {
    "/api": {
        "target": process.env['VITE_BFF_URL'],
        "secure": false,
        "changeOrigin": true,
        "pathRewrite": {
            "^/api": ""
        }
    }
}

export default PROXY_CONFIG;