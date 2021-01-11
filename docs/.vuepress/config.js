module.exports = {
    title: 'Hero权限管理系统在线文档',
    description: '该文档描述基于surging.cloud实现的权限管理系统',
    port: 8081,
    themeConfig: {
        nav: [
            { text: '首页', link: '/' },
            { text: '服务端', link: '/hero/' },
            { text: '前端', link: '/hero/web/' },
            {
                text: 'github',
                items: [
                    { text: 'hero服务端', link: 'https://github.com/liuhll/hero' },
                    { text: 'hero前端', link: 'https://github.com/liuhll/hero-web' }
                ]
            },
        ],
        sidebar: {
            '/hero/': [
                {
                    title: '简介',
                    collapsable: false,
                    children: [
                        ''
                    ]

                },                
                {
                    title: '开发环境',
                    collapsable: false,
                    children: [
                        'development-env'
                    ]

                },
                {
                    title: '开发文档',
                    collapsable: false,
                    children: [
                        'dev-docs/structure',
                        'dev-docs/appserver',
                    ]
                }
            ]
        }
    }
}