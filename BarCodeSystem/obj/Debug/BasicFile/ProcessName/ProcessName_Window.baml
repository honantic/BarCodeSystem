   M S B A M L   `   `   `  ����H  DBarCodeSystem, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:BarCodeSystemBarCodeSystem  5   	   &     BarCodeSystem.ProcessName_Window   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   /namespcclr-namespace:BarCodeSystem     5   	   -   5          � WindowStartupLocation$  CenterScreen=�5   	    � 
ResizeMode$ NoResize=�5   	    � Title$ 工序中心��5   	   $	��800��6   $	��600��6#    � Icon$. (/BarCodeSystem;component/Images/burn.ico��6F    � 	Resources 5      -   'BarCodeSystem.ListViewItemStyleSelector   
mySelector  	FirstCell �� Orientation � Cursor d� 	IsChecked �� Path��%�   &	        &	        5	   
   6,   �� 5
   
   .R�5      �� $ 
Horizontal=�6   $��Stretch=�62   .+�5      �� $
 Hand��6B    6   �� $ 
IsSelected�6B   5      5      5      .��5      � 	 � RowDefinitions	 5   
   �� 5      $��60 �6   6*   �� 5      6   �� 5      $��60 �6   6*   .+�5   
   �� -   5        $��	listview1��5   +   $��1��6   $��5q�6    #���   5      
 �� View
 5      �� 5      .P�5      �� $��选择��6%    �� CellTemplate# [ 61    �� Width$ 30��6[   6h   �� 5      $��工序编码��6%   $
 Auto��6a    �� DisplayMemberBinding 63   �� $ PN_Code�6p   �� 5      $��工序名称��6%   $
 Auto��6a    63   �� $ PN_Name�6p   �� 5      $��	工序号��6%   $
 Auto��6l    62   �� $ PN_CodeInWorkCenter�6|   5      5!      �� 5"   
   $��2��6   $ 
Horizontal=�6"   .+�5#      �� -   5        $��btn_Add��5#      $��30��6:   $��5q�6G   $��
  新增  ��6R   6e   �� 5$      -   5        $��
btn_Modify��5$      $��30��6@   $��5q�6L   $��	 修改  ��6W   6i   �� 5%      -   5        $��
btn_Delete��5%      $��30��6@   $��5q�6L   $��	 删除  ��6W   6i   �� 5&      -   5        $��
btn_Import��5&      $��30��6@   $��5q�6L   $��Excel导入信息��6W   6n   �� 5'      -   5        $��btn_SelectAll��5'      $��30��6F   $��5q�6R   $��40��6]   $��全选��6h   6w   �� 5(      -   5        $��btn_ReSelect��5(      $��30��6D   $��5q�6P   $��40��6[   $��重选��6f   6u   �� 5)      -	   5        $��txtb_SearchKey��5)      $��30��6U   $��80��6a   $��14(�6l   $��5q�6z   6�   �� 5*      -
   5        $��
btn_Search��5*      $��30��6@   $��40��6L   $��0 5 5 5q�6W   $��搜索��6h   6w   �� 5+      -   5        $��	btn_Close��5+      $��30��6>   $��80��6J   $��5q�6U   $��关闭��6`   6o   5,      5-      5.      