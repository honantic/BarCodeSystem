   M S B A M L   `   `   `  ����H  DBarCodeSystem, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:BarCodeSystemBarCodeSystem  5   	   '    !BarCodeSystem.PersonImport_Window   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   /namespcclr-namespace:BarCodeSystem     5   	   -   5          � WindowStartupLocation$  CenterScreen=�5   	    � 
ResizeMode$ NoResize=�5   	    � Title$ 人员导入窗口��5   	   $	��600��6   $	��600��6%    � Icon$. (/BarCodeSystem;component/Images/burn.ico��6a    � 	Resources 5         BarCodeSystem.PersonLists   
mySelector��%   &	          5	   
   6   5
      .��5      �  � RowDefinitions 5   
   �� 5      $��60 �6   6*   �� 5      6   �� 5      $��60 �6   6*   .+�5   
   �� $��2��6    �� Orientation$ 
Horizontal=�6"   $��Right=�6;   .+�5      �� -   5        $��btn_Copy��5      $��复制数据��6<    � ToolTip$ Ctr+V��6K   $��5 5 80 5q�6[   $��30��6m   $��80��6y   6�   �� 5      -   5        $��
btn_Import��5      $��导入条码系统��6@   $! 将数据导入条码系统��6Q   $��5 5 80 5q�6e   $��30��6w   $��80��6�   6�   �� 5      -   5        $��btn_OutPrint��5      $��导出数据��6)   $��30��6T   $��80��6`   $��5 5 80 5q�6k   6   �� 5      -   5        $��	btn_Close��5      $��关闭��6>   $��5q�6K   $��30��6V   $��80��6b   6p   5      �� 5   
   -   5        $��	listview1��5   +   $��1��6   $��5q�6    #���   5      ��5   <   ��  �� View 5      �� 5      .P�5      �� $��员工编号��6%   	 �� Width$		 100��6^   
 �� DisplayMemberBinding
 63   ��  �� Path$
 code�6l   �� 5      $��员工姓名��6%   $		 100��6^   
 63   �� $
 name�6l   �� 5      $��岗位��6%   $		 100��6`   
 61   �� $ position�6n   �� 5      $��工作中心编号��6%   $		 150��6f   
 65   �� $ 
departCode�6t   �� 5      $��工作中心名称��6%   $		 150��6f   
 65   �� $ 
departName�6t   5       5"      5#      5$      