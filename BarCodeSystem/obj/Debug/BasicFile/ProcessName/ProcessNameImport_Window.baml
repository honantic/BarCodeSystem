   M S B A M L   `   `   `  ����H  DBarCodeSystem, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:BarCodeSystemBarCodeSystem  5   	   ,    &BarCodeSystem.ProcessNameImport_Window   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   /namespcclr-namespace:BarCodeSystem     5   	   -   5          � 
ResizeMode$  NoResize=�5   	    � WindowStartupLocation$ CenterScreen=�5   	    � Title$ 工序导入窗口��5   	   $	��600��6   $	��600��6%    � Icon$. (/BarCodeSystem;component/Images/burn.ico��6J    � 	Resources 5	      -   'BarCodeSystem.ListViewItemStyleSelector   
mySelector  	FirstCell �� Orientation � Cursor d� 	IsChecked �� Path��%�   &	        &	        5
   
   6,   �� 5   
   .R�5      �� $ 
Horizontal=�6   $��Stretch=�62   .+�5      �� $
 Hand��6B    6   �� $ 
IsSelected�6B   5      5      5      .��5      � 	 � RowDefinitions	 5   
   �� 5      $��60 �6   6*   �� 5      6   �� 5      $��60 �6   6*   .+�5   
   �� -   5        $��	listview1��5   +   $��1��6   $��5q�6    #���   5      
 �� View
 5      �� 5      .P�5      �� $��选择��6%    �� CellTemplate# [ 61    �� Width$ 30��6[   6h   �� 5      $��工序编号��6%   $	 200��6a    �� DisplayMemberBinding 63   �� $ PN_Code�6o   �� 5      $��工序名称��6%   $	 200��6a    63   �� $ PN_Name�6o   5       5"      �� 5$   
   $��2��6   $ 
Horizontal=�6"   $��Right=�6;   .+�5%      �� -   5        $��btn_Copy��5%      $��复制数据��6<    � ToolTip$ Ctr+V��6K   $��5 5 80 5q�6[   $��30��6m   $��80��6y   6�   �� 5&      -   5        $��
btn_Import��5&      $��导入条码系统��6@   $! 将数据导入条码系统��6Q   $��5 5 80 5q�6e   $��30��6w   $��80��6�   6�   �� 5'      -   5        $��btn_OutPrint��5'      $��导出数据��6)   $��30��6T   $��80��6`   $��5 5 80 5q�6k   6   �� 5(      -   5        $��	btn_Close��5(      $��关闭��6>   $��5 5 10 5q�6K   $��30��6]   $��80��6i   6w   5)      5+      5,      