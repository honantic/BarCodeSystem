   M S B A M L   `   `   `  ����H  DBarCodeSystem, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:BarCodeSystemBarCodeSystem  5   	   -    'BarCodeSystem.WorkTeamDepartList_Window   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   /namespcclr-namespace:BarCodeSystem     5   	   -   5          � WindowStartupLocation$  CenterScreen=�5   	    � 
ResizeMode$ NoResize=�5   	    � Title$ 工作中心清单��5   	   $	��600��6   $	��400��6%    � Icon$. (/BarCodeSystem;component/Images/burn.ico��6H    � 	Resources 5      -   'BarCodeSystem.ListViewItemStyleSelector   
mySelector��%   &	          5	   
   6,   5
      .��5      �  � RowDefinitions 5   
   �� 5      $��60 �6   6*   �� 5      6   �� 5      $��60 �6   6*    � ColumnDefinitions 5   
   �� 5      6!   �� 5      6!   .+�5   
   �� -   5        $��lv_listview1��5      $��1��6'   $��2��64   $��5q�6H   #���   5      ��6M   ��  �� View 5      �� 5      .P�5      �� $��工作中心编码��6%    �� Width$	 180��65   	 �� DisplayMemberBinding	 6A   �� 
 �� Path$
 department_code�6y   �� 5      $��工作中心名称��6%   $	 180��65   	 6A   �� $
 department_name�6y   5      5      �� 5   
   -   5        $��	btn_Chose��5      $��2��6:   $��30��6G   $��80��6S   $��选择��6^   6m   �� 5   
   -   5        $��	btn_Close��5      $��2��6:   $��1��6G   $��30��6W   $��80��6c   $��关闭��6n   6}   5       5!      