   M S B A M L   `   `   `  ����H  DBarCodeSystem, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:BarCodeSystemBarCodeSystem  5   	   e7clr-namespace:BarCodeSystem.PublicClass.ValueConverters)BarCodeSystem.PublicClass.ValueConverters  5   	   (    "BarCodeSystem.QualityIssues_Window   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   /namespcclr-namespace:BarCodeSystem     5   	   Ilocal7clr-namespace:BarCodeSystem.PublicClass.ValueConverters     5   	   -   5          � WindowStartupLocation$  CenterScreen=�5   	    � 
ResizeMode$ NoResize=�5   	    � Title$ 质量问题信息中心��5   	   $	��800��6   $	��600��6'    � Icon$. (/BarCodeSystem;component/Images/burn.ico��6J    � 	Resources 5	      -   'BarCodeSystem.ListViewItemStyleSelector   
mySelector  	FirstCell �� Orientation � Cursor d� 	IsChecked �� PathI   CBarCodeSystem.PublicClass.ValueConverters.QualityIssueTypeConverter  QITypeConverter��%�   &	        &	      &	 �       5
   
   6,   �� 5   
   .R�5      �� $ 
Horizontal=�6   $��Stretch=�62   .+�5      �� $
 Hand��6B    6   �� $ 
IsSelected�6B   5      5        5   
   6D   5      .��5      � 	 � RowDefinitions	 5   
   �� 5      $��60 �6   6*   �� 5      6   �� 5      $��60 �6   6*   .+�5   
   �� -   5        $��	listview1��5   +   $��1��6   $��5q�6    #���   5      
 �� View
 5      �� 5      .P�5      �� $��选择��6%    �� CellTemplate# [ 61    �� Width$ 30��6[   6h   �� 5      $��质量问题编码��6%   $
 Auto��6c    �� DisplayMemberBinding 65   �� $ QI_Code�6r   �� 5      $��质量问题原因��6%   $
 Auto��6c    65   �� $ QI_Name�6r   �� 5       $��质量问题类型��6%   $
 Auto��6�    65   �� $ QI_Type� �� 	Converter# [ 6�   �� 5!      $��质量问题条码��6%   $
 Auto��6f    65   �� $ 
QI_BarCode�6u   5"      5$      � 5%   
   $��2��6    � ColumnDefinitions 5&      �� 5'      6%   �� 5(      6%   �� 5)      6%   �� 5*      6&   �� 5+      6%   �� 5,      6%   �� 5-      $��2* �6#   60   �� 5.      6%   .+�50      �� -   5        $��btn_Add��50      $��30��6:   $��0��6F   $��Center=�6V   $��
  新增  ��6s   6�   �� 51      -   5        $��
btn_Delete��51      $��30��6@   $��Center=�6L   $��   删除  ��6i   $��2��6{   6�   �� 52      -   5        $��	btn_Print��52      $��30��6>   $��
  打印  ��6J   $��Center=�6[   $��3��6x   6�   �� 53      -   5        $��
btn_Modify��53      $��30��6@   $��1��6M   $��Center=�6]   $y�Visible=�6z   $��
  修改  ��6�   6�   �� 54      -   5        $��
btn_Import��54      $y�Hidden=�6@   $��30��6T   $��Center=�6`   $��3��6~   $��Excel导入信息��6�   6�   �� 55      -   5        $��btn_SelectAll��55      $��30��6F   $��4��6S   $��40��6d   $��全选��6o   6~   �� 56      -	   5        $��btn_ReSelect��56      $��30��6D   $��5��6Q   $��40��6b   $��重选��6m   6|   �� 57      $��6��6   $ 
Horizontal=�6*   .+�58      �� -
   5        $��txtb_SearchKey��58      $��30��6Y   $��10q�6e   $��80��6q   $��14(�6|   6�   �� 59      -   5        $��
btn_Search��59      $��30��6D   $��搜索��6R   6a   5:      �� 5<      -   5        $��	btn_Close��5<      $��7��6'   $��30��6O   $��Center=�6[   $��
  关闭  ��6x   6�   5=      5>      5?      