
import { createRouter, createWebHashHistory } from 'vue-router'
import routes from './routes'

// 创建一个可以被 Vue 应用使用的 Router 实例。
const router = createRouter({
  history: createWebHashHistory(),  // 路由器使用的历史记录模式
  routes,                           // 添加到路由器的初始路由列表
})

/**
 * 全局后置钩子
 * 
 */
router.afterEach((to, from, failure) => {
  const { title } = to.meta                 // 获取跳转页面的meta为要设置的当前页面的title
  document.title = title ? `${title}` : "websiteTitle"  // 修改当前页面的title
})

/**
 * 全局前置路由守卫
 * to: Route: 即将要进入的目标 路由对象
 * from: Route: 当前导航正要离开的路由
 * next: Function: 一定要调用该方法来 resolve 这个钩子。执行效果依赖 next 方法的调用参数。
 */
router.beforeEach((to, from, next) => {
  if (to.path === '/login') return next() // 判断是否登录页面，登录页面直接执行next()
  // 获取token
  const tokenStr = window.sessionStorage.getItem('token')
  if (!tokenStr) return next('/login')  // 没有获取到token,返回登录页面
  next()
})

export default router
