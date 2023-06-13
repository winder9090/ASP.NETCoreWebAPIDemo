import type { RouteRecordRaw } from 'vue-router'
import Login from "../../views/Login.vue"

/**
 * 路由配置
 * @description 所有路由都在这里集中管理
 */
const routes: RouteRecordRaw[] = [
    {
        path: '/',
        redirect: '/Login',
        // name: 'Home',
        // component: () => import('@/views/home.vue'),
        // meta: {
        //   title: 'Home',
        // },
        },
        {
        path: '/Login',
        name: 'Login',
        component: Login,
        meta: {
            title: 'Login',
        },
    },
]

export default routes
