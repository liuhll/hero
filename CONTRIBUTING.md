# 开发指南

## 开发流程
项目使用git工作流开发和管理项目。

如下所述，简要的描述了git工作流：

1. 使用git clone项目源代码;

2. 新建本地分支，开发新特性或修复bug；

3. 提交代码到本地仓库；

4. 通过`git fetch` 拉取最新代码，并使用`git rebase`命令与远程仓库的 develop 分支进行合并(变基),如果存在冲突，请解决冲突。

5. push代码到远程仓库,如果是新功能开发，远程分支请以`feature-developernane-function`命名,如果是修复bug请以`bug-issueid`命名；

6. 在gitlab上发起pull request,并指派给项目责任人。

更多关于git工作流知识，请参考：
- [阮一峰:Git 工作流程](http://www.ruanyifeng.com/blog/2015/12/git-workflow.html)
- [博客园:Git工作流指南：Gitflow工作流](https://www.cnblogs.com/jiuyi/p/7690615.html)