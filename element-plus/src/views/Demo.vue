<!-- eslint-disable vue/multi-word-component-names -->
// eslint-disable-next-line vue/multi-word-component-names
<template>
    <div class="homeBox">
        <!-- 外层容器 -->
        <el-container style="height:100%;padding:0;margin:0;width:100%;" direction="vertical">
            <!-- 顶栏容器 -->
            <el-header style="text-align: right; font-size: 12px">
                <div>
                    <img src="../assets/logo.png" style="height: 24px;margin-right: 8px; " />
                    <span style="margin-right: 8px; margin-top: 1px;font-size: 24px; color:aliceblue">xx管理系统</span>
                </div>
                <div>
                    <el-dropdown>
                        <el-icon style="margin-right: 8px; margin-top: 1px; color:aliceblue;margin-right: 8px; "><i
                                class="el-icon-setting" /></el-icon>
                        <template #dropdown>
                            <el-dropdown-menu>
                                <el-dropdown-item @click="Logout">logout</el-dropdown-item>
                            </el-dropdown-menu>
                        </template>
                    </el-dropdown>
                    <span style="; color:aliceblue">Tom</span>
                </div>
            </el-header>

            <!-- 内容主体区域 -->
            <el-container>
                <!-- 侧边栏 -->
                <el-aside :width="isCollapse ? '64px' : '200px'">
                    <div class="toggle-button" @click="toggleCollapse"><el-icon :size="32">
                            <Expand v-show="isCollapse" />
                            <Fold v-show="!isCollapse" />
                        </el-icon></div>

                    <!-- 侧边栏菜单区域 -->
                    <!-- router：是否启用 vue-router 模式。 启用该模式会在激活导航时以 index 作为 path 进行路由跳转 -->
                    <!-- :unique-opened：是否只保持一个子菜单的展开 -->
                    <!-- :collapse：是否水平折叠收起菜单（仅在 mode 为 vertical 时可用） -->
                    <!-- :collapse-transition：是否开启折叠动画 -->
                    <el-menu :default-openeds="['1']" router background-color="#333744" text-color="#fff"
                        active-text-color="#409eff" :unique-opened="true" :collapse="isCollapse"
                        :collapse-transition="false">
                        <el-sub-menu index="1">
                            <!-- 一级菜单模板区域 -->
                            <template v-slot:title>
                                <!-- 图标 -->
                                <!-- <el-icon><i class="el-icon-menu"/></el-icon> -->
                                <!-- 文本 -->
                                <span>主菜单</span>
                            </template>

                            <!-- 二级菜单 -->
                            <el-sub-menu index="2">
                                <!-- 二级菜单模板区域 -->
                                <template v-slot:title>
                                    <i class="el-icon-user"></i>
                                    Basic 基础组件
                                </template>
                                <el-menu-item index="/Demo/Button"> <i class="el-icon-user"></i>Button 按钮</el-menu-item>
                            </el-sub-menu>
                        </el-sub-menu>
                    </el-menu>
                </el-aside>

                <!-- 右侧内容主体区域 -->
                <el-main>

                    <Suspense>
                        <router-view />
                    </Suspense>

                </el-main>
            </el-container>
        </el-container>
    </div>
</template>

<script lang="ts" setup>
import {
    Menu as IconMenu,
    MapLocation,
} from '@element-plus/icons-vue'
// import { defineComponent } from "vue";
import { ref } from "vue";
import router from "../router";
// import { Menu as IconMenu,Setting } from '@element-plus/icons'

// export default defineComponent ({
//   setup() {
//     const isCollapse = ref(false);

//     const toggleCollapse = () => {
//       // 点击按钮，切换菜单的折叠与展开
//       isCollapse.value = !isCollapse.value;
//     };

//     const setting =ref(Setting) ;
//     const iconMenu =IconMenu ;
//     return {isCollapse,toggleCollapse,setting,iconMenu};
//   }
// });

const isCollapse = ref(false);
// const setting =ref(Setting) ;
// const iconMenu =IconMenu ;

const toggleCollapse = () => {
    // 点击按钮，切换菜单的折叠与展开
    isCollapse.value = !isCollapse.value;
};

const Logout = () => {
    window.sessionStorage.clear()
    router.push('/login')
}

</script>

<style scoped>
.el-main {
    padding-left: 20px;
    padding-right: 20px;
    padding-top: 20px;
    padding-bottom: 20px;
}

.homeBox {
    /*设置内部填充为0,几个布局元素之间没有间距*/
    padding: 0px;

    /*外部间距也是如此设置*/
    margin: 0px;

    /*统一设置高度为100%*/
    height: 100%;

}

.el-header {
    background-color: #373d41;
    /* 给头部设置一下弹性布局 */
    display: flex;
    /* 让它贴标左右对齐 */
    justify-content: space-between;
    /* 清空图片左侧padding */
    padding-left: 0;
    /* 按钮居中 */
    align-items: center;

    /* 嵌套 */
    >div {
        /* 弹性布局 */
        display: flex;
        /* 纵向上居中对齐 */
        align-items: center;

        img {
            width: 60px;
            border-radius: 50%;
        }

        ;

        /* 给文本和图片添加间距，使用类选择器 */
        span {
            margin-left: 15px;
        }
    }

    ;
}

.el-aside {
    background-color: #333744;
}

/*左侧菜单伸缩按钮样式*/
.toggle-button {
    font-size: 10px;
    line-height: 24px;
    color: #fff;
    text-align: right;
    letter-spacing: 0.2em;
    cursor: pointer;
}

.el-icon {
    vertical-align: middle;
    margin-right: 5px;
    width: 24px;
    text-align: center;
    font-size: 18px;
}</style>