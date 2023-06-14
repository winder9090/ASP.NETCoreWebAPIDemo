import type { RouteRecordRaw } from 'vue-router'
import Login from "../../views/Login.vue"
import Demo from "../../views/Demo.vue"
import HelloWorld from "../../components/HelloWorld.vue"
import Button from "../../components/Button.vue"

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
    {
        path: '/Demo',
        name: 'Demo',
        component: Demo,
        redirect: '/HelloWorld',
        meta: {
          title: 'Demo',
        },
        children: [
          { path: "/HelloWorld", component: HelloWorld },
          { path: "Button", component: Button },
          
        ],
      },
]

export default routes
