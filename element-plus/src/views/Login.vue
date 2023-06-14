<!-- eslint-disable vue/multi-word-component-names -->
// eslint-disable-next-line vue/multi-word-component-names
<template>
    <div class="login_container">
        <div class="login_box">
            <div class="title_box">
                <img src="../assets/logo1.png" alt=""/>
            </div>
            <!-- 头像区域 -->
            <div class="avatar_box">
                <img src="../assets/logo.png" alt=""/>
            </div>
            <!-- 登录表单区域 用户名:admin 密码:123456 -->
            <el-form ref="loginFormRef" :model="loginForm" :rules="loginFormRules" label-width="0px" class="login_form">
                <!-- 用户名 -->
                <el-form-item prop="username">
                    <el-input v-model="loginForm.username" prefix-icon="User"></el-input>
                </el-form-item>
                <!-- 密码 -->
                <el-form-item prop="password">
                    <el-input v-model="loginForm.password" type="password" prefix-icon="Lock"></el-input>
                </el-form-item>
                <!-- 按钮区域 -->
                <el-row justify="end">
                    <el-form-item class="btns">
                        <el-button type="primary" @click="Login">登录</el-button>
                        <el-button type="info" @click="resetLoginForm">重置</el-button>
                    </el-form-item>
                </el-row>
            </el-form>
        </div>
    </div>
</template>

<script>
    export default{
        setup (){
            window.sessionStorage.clear()
        },
        data() {
            return{
                loginForm: {
                    username: '',
                    password: '' 
                },
                loginFormRules:{
                    // 验证用户名是否合法
                    username:[
                        {required:true,message:'请输入用户名称',trigger:'blur'},
                        {min:3,max:5,message:'长度在 3 到 5 个字符',trigger:'blur'}
                    ],
                    // 验证密码是否合法
                    password:[
                        {required:true,message:'请输入登录密码',trigger:'blur'},
                        {min:5,max:15,message:'长度在 5 到 15 个字符',trigger:'blur'}
                    ]
                }
            }
        },
        methods: {
            // 重置按钮事件
            resetLoginForm () {
                this.$refs.loginFormRef.resetFields()
            },
            Login () {
                this.$refs.loginFormRef.validate(async valid => {
                    if(!valid) return;
                    // const { data: result } = await this.$http.post('login', this.loginForm)
                    // if (result.meta.status !== 200) return this.$message.error('登录失败')
                    // this.$message.success('登录成功')
                    // console.log(result.data.token)
                    // window.sessionStorage.setItem('token', result.data.token)

                    this.$message.success('登录成功')
                    window.sessionStorage.setItem('token', "123")
                    this.$router.push('/Demo')
                })
            }
        }
    }
</script>

<style lang="less" scoped>
    .login_container{
        background-color: #2b4b6b;
        height: 100%;
    }

    .login_box{
        // 宽450像素
        width: 450px;
        // 高300像素
        height: 300px;
        // 背景颜色
        background-color: #fff;
        // 圆角边框3像素
        border-radius: 3px;
        // 绝对定位
        position: absolute;
        // 距离左侧50%
        left: 50%;
        // 上面距离50%
        top: 50%;
        // 进行位移，并且在横轴上位移-50%，纵轴也位移-50%
        transform: translate(-50%,-50%);

        .title_box{
            // 绝对定位
            position: absolute;
            // 距离左侧50%
            left: 50%;
            // 进行位移，并且在横轴上位移-50%，纵轴位移-300%
            transform: translate(-50%,-300%);
            img{
                width: 100%;
                height: 100%;
            }
        }

        .avatar_box{
            // 盒子高度130px
            height: 130px;
            // 盒子宽度103px
            width: 130px;
            // 边框线1像素 实线
            border: 1px solid #eee;
            // 圆角
            border-radius: 50%;
            // 内padding
            padding: 10px;
            // 添加阴影 向外扩散10像素 颜色ddd
            box-shadow: 0 0 10px #ddd;
            // 绝对定位
            position: absolute;
            // 距离左侧50%
            left: 50%;
            // 进行位移，并且在横轴上位移-50%，纵轴也位移-50%
            transform: translate(-50%,-50%);
            // 背景色
            background-color: #fff;
            img{
                width: 100%;
                height: 100%;
                // 圆角
                border-radius: 50%;
                background-color: #eee;
            }
        }
    }

    .btns{
        // 设置弹性模式
        display: flex;
        // 横轴上尾部对齐
        justify-content: flex-end;
    }

    .login_form{
        position: absolute;
        bottom: 0;
        width: 100%;
        padding: 0 20px;
        box-sizing: border-box;
    }
</style>