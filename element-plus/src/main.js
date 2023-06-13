import { createApp } from 'vue'
import App from './App.vue'
import './index.scss'
import installElementPlus from './plugins/element'
import router from './router/index'
import locale from 'element-plus/lib/locale/lang/zh-cn'
import { ElMessage } from 'element-plus'
import * as ElementPlusIconsVue from '@element-plus/icons-vue'

const app = createApp(App)
app.use(router)
installElementPlus(app)
for (const [key, component] of Object.entries(ElementPlusIconsVue)) {
    app.component(key, component)
}
app.mount('#app')
